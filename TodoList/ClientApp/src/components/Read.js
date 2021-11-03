import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Select } from './Select';

export class Read extends Component {
    static displayName = Read.name;

    mounted = true;

    page = 0;

    expired = "#333333";

    // NB: костыль для старта, '6' - отсутствие фильтрации на бэке
    filter = 6;

    constructor(props) {
        super(props);
        this.state = { backlog: [], problemStatuses: [], loading: true };
    }

    async componentDidMount() {
        await this.getProblemStatus();
        await this.getEntriesData();
        this.mounted = true;
    }

    componentWillUnmount() {
        this.mounted = false;
    }

    back = () => {
        this.page--;
        this.getEntriesData();
    }

    forw = () => {
        this.page++;
        this.getEntriesData();
    }

    select = (e) => {
        // NB: id в бд начинаются от единицы
        this.filter = Number(e.target.value) + 1;
        this.getEntriesData();
    }

    //expired = {
    //    backgroundColor: "#333333"
    //}

    checkValidity = (backlog) => {
        // TODO: сделай валидацию по дэдлайну
        return false;
    }

    renderBacklogTable(backlog) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel" id="theme">
                <thead>
                    <tr key={"button"}>
                        <th>
                            <button onClick={this.back} className="btn btn-info">&lt;BACK</button>
                        </th>
                        <th>
                            <button onClick={this.forw} className="btn btn-info">FORW&gt;</button>
                        </th>
                        <th>
                            <select onChange={this.select} value={ Number(this.filter - 1) }>
                                {this.state.problemStatuses.map((a, i) =>
                                    <option value={i} key={i.toString()}>
                                        {a.problemStatusName}
                                    </option>
                                )}
                            </select>
                        </th>
                    </tr>
                    <tr>
                        <th>Title</th>
                        <th>Initiator</th>
                        <th>Executor</th>
                        <th>Start Date</th>
                        <th>Deadline</th>
                        <th>Completion Date</th>
                    </tr>
                </thead>
                <tbody>
                    {backlog.map(backlog =>
                        <React.Fragment key={backlog.entryId}>
                            <tr style={{ backgroundColor: this.checkValidity(backlog) === true ? "white" : this.expired }}>
                                <td>{backlog.title}</td>
                                <td>{backlog.initiator.name}</td>
                                <td>{backlog.executor.name}</td>
                                <td>{new Date(backlog.startDate).toDateString()}</td>
                                <td>{new Date(backlog.deadline).toDateString()}</td>
                                <td>{new Date(backlog.completionDate).toDateString()}</td>
                            </tr>
                            <tr>
                                <th colSpan="2" scope="row">Description</th>
                                <td colSpan="3" style={{ display: '' }}>
                                    {backlog.description.substring(0, 80) + "..."}
                                </td>
                                <td>
                                    <Link to={{
                                        pathname: '/update',
                                        propsState: backlog.entryId,
                                        fromReadComponent: this.page
                                    }}>
                                        <button className="btn btn-info">UPDATE</button>
                                    </Link>
                                </td>
                            </tr>
                        </React.Fragment>
                    )}

                </tbody>
            </table>
        );
    }

    // display: 'none' display: ''
    //style = {{
    //                ...styles.productOptions,
    //    backgroundColor: checkedButton === item.id ? "grey" : "white",
    //              }}
    render() {
        let contents = this.state.loading
            ? <p><em>Please wait...</em></p>
            : this.renderBacklogTable(this.state.backlog);

        return (
            <div>
                <h1 id="tabelLabel" >Todo List</h1>
                <p>This component is under development.</p>
                {contents}
            </div>
        );
    }

    async getEntriesData() {
        if (this.props.location.fromUpdateComponent != undefined) {
            this.page = this.props.location.fromUpdateComponent;
            this.filter = this.props.location.filter;
            this.props.location.fromUpdateComponent = undefined;
        }

        const response = await fetch('entry/ongetpage?page=' + this.page + "&filter=" + this.filter);
        const data = await response.json();

        if (data != null && data.length > 0) this.page = data[0].currentPage;

        if (this.mounted) this.setState({ backlog: data, loading: false });
    }

    async getProblemStatus() {
        const response = await fetch('entry/ongetproblemstatuses');
        const data = await response.json();
        if (this.mounted) this.setState({ problemStatuses: data });
    }
}