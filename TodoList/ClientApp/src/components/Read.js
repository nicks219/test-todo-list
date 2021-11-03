import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Header, TableBody, Select } from './Table';
import { RenderContent, getProblemStatuses } from './Loader';

export class Read extends Component {
    displayName = Read.name;

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
        await getProblemStatuses(this);
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
        this.filter = Number(++e.target.value);
        this.getEntriesData();
    }

    checkValidity = (backlog) => {
        // TODO: сделай валидацию по дэдлайну или удали этот метод
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
                            <Select select={this.select} value={this.filter} list={this.state.problemStatuses} id={1} />
                        </th>
                    </tr>
                    <tr>
                        <Header />
                    </tr>
                </thead>
                <tbody>
                    {backlog.map(backlog =>
                        <React.Fragment key={backlog.entryId}>
                            <tr style={{ backgroundColor: this.checkValidity(backlog) === true ? "white" : this.expired }}>
                                <TableBody backlog={backlog} />
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

    render() {

        return (
            <RenderContent component={this} />
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
        if (data != null && data.length > 0) {
            this.page = data[0].currentPage;
        }

        if (this.mounted) {
            this.setState({ backlog: data, loading: false });
        }
    }
}

// display: 'none' display: ''
//style = {{
//                ...styles.productOptions,
//    backgroundColor: checkedButton === item.id ? "grey" : "white",
//              }}