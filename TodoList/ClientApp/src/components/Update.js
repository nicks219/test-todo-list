import { data } from 'jquery';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Update extends Component {
    static displayName = Update.name;
    id = 0;
    page = 0;

    constructor(props) {
        super(props);
        this.state = { backlog: [], problemStatuses: [], loading: true };
        // костыль: при "обновлении" будет загружена первая запись, но хотя бы не "отвалится"
        if (props.location.fromReadComponent != undefined) {
            this.id = props.location.propsState;
            this.page = props.location.fromReadComponent;
        }
    }

    componentDidMount() {
        this.getProblemStatus();
        this.getEntriesData();
    }

    update = () => {
        this.putEntriesData();
    }

    select = (e) => {

        const data = this.state.backlog;
        const status = Number(e.target.value) + 1;
        data.taskStatus.problemStatusId = status;
        this.setState({ backlog: data });
    }

    expired = {
        backgroundColor: "#FF0000"
    }

    checkVailidity = (backlog) => {
        return false;
    }

    renderBacklogTable(backlog) {

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr key={"button"}>
                        <th>
                            <button onClick={this.update} className="btn btn-info">UPDT</button>
                        </th>
                        <th>
                            <Link to={{ pathname: '/', fromUpdateComponent: this.page }}>
                                <button className="btn btn-info">RTRN</button>
                            </Link>
                        </th>
                    </tr>
                    <tr>
                        <th>Title</th>
                        <th>Initiator</th>
                        <th>Executor</th>
                        <th>Start Date</th>
                        <th>Deadline</th>
                        <th>Completion Date</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>

                    <React.Fragment key={backlog.entryId}>
                        <tr style={{ backgroundColor: this.checkVailidity(backlog) == true ? "white" : "red" }}>
                            <td>{backlog.title}</td>
                            <td>{backlog.initiator.name}</td>
                            <td>{backlog.executor.name}</td>
                            <td>{backlog.deadline}</td>
                            <td>{backlog.startDate}</td>
                            <td>{backlog.completionDate}</td>
                            <td>
                                <select onChange={this.select} value={Number(backlog.taskStatus.problemStatusId - 1)}>
                                    {this.state.problemStatuses.map((a, i) =>
                                        <option value={i} key={i.toString()}>
                                            {a.problemStatusName}
                                        </option>
                                    )}
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <th colSpan="2" scope="row">Description</th>
                            <td colSpan="3" style={{ display: '' }}>
                                <textarea id={8} value={backlog.description} cols={66} rows={8} onChange={this.inputText} />
                            </td>
                        </tr>
                    </React.Fragment>

                </tbody>
            </table>
        );
    }

    inputText = (e) => {
        const newText = e.target.value;
        const data = this.state.backlog;
        data.description = newText;
        this.setState({ backlog: data });
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Please wait...</em></p>
            : this.renderBacklogTable(this.state.backlog);

        return (
            <div>
                <h1 id="tabelLabel" >Backlog</h1>
                <p>This component is under development.</p>
                {contents}
            </div>
        );
    }

    async getEntriesData() {
        const response = await fetch('entry/ongetentry?id=' + this.id);
        const data = await response.json();
        this.setState({ backlog: data, loading: false });
    }

    async putEntriesData() {
        var item = { EntryId: 1 };
        var requestBody = JSON.stringify(item);
        requestBody = { "EntryId": 1 };
        requestBody = JSON.stringify(this.state.backlog);

        const response = await fetch('entry',
            { method: "PUT", headers: { 'Content-Type': "application/json;charset=utf-8" }, body: requestBody });
        const data = await response.json();
        this.setState({ backlog: data, loading: false });
    }

    async getProblemStatus() {
        const response = await fetch('entry/ongetproblemstatuses');
        const data = await response.json();
        this.setState({ problemStatuses: data });
    }
}