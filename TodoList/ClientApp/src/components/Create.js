import React, { Component, useState } from 'react';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

// TODO: учись писать функциональные компоненты на хуках, вот пример моего календаря:
const Example = () => {
    const [startDate, setStartDate] = useState(new Date());
    return (
        <DatePicker selected={startDate} onChange={(date) => setStartDate(date)} />
    );
};

export class Create extends Component {
    static displayName = Create.name;

    mounted = false;

    id = 0;

    page = 0;

    filter = 0;

    expired = "#333333";

    constructor(props) {
        super(props);
        this.state = { backlog: [], problemStatuses: [], users: [], date: new Date(), loading: true };
        // костыль: при "обновлении" будет загружена первая запись, но хотя бы не "отвалится"
        if (props.location.fromReadComponent != undefined) {
            this.id = props.location.propsState;
            this.page = props.location.fromReadComponent;
        }
    }

    componentDidMount() {
        this.getProblemStatus();
        this.getUsers();
        this.getEntriesData();
        this.mounted = true;
    }

    componentWillUnmount() {
        this.mounted = false;
    }

    create = () => {
        this.postEntriesData();
    }

    select = (e) => {

        const data = this.state.backlog;
        const number = Number(e.target.value) + 1;
        const id = Number(e.target.id);

        if (id === 1) data.taskStatus.problemStatusId = number;
        if (id === 0) data.initiator.userId = number;

        this.setState({ backlog: data });
    }

    //expired = {
    //    backgroundColor: "#333333"
    //}

    checkValidity = (backlog) => {
        return false;
    }

    setStartDate = (dt) => {
        const data = this.state.backlog;
        data.startDate = dt.toISOString();
        data.deadline = dt.toISOString();
        this.setState({ date: dt, backlog: data });
    }

    renderBacklogTable(backlog) {

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel" id="theme">
                <thead>
                    <tr key={"button"}>
                        <th>
                            <button onClick={this.create} className="btn btn-info">CREATE</button>
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
                        <tr style={{ backgroundColor: this.checkValidity(backlog) === true ? "white" : this.expired }}>
                            <td>{backlog.title}</td>
                            <td>

                                <select onChange={this.select} value={Number(this.state.backlog.initiator.userId - 1)} id={0}>
                                    {this.state.users.map((a, i) =>
                                        <option value={i} key={i.toString() + 'i'}>
                                            {a.name}
                                        </option>
                                    )}
                                </select>

                            </td>
                            <td>
                                {backlog.executor.name}
                            </td>
                            <td>
                                <DatePicker selected={this.state.date} onChange={(date) => this.setStartDate(date)} />
                            </td>
                            <td>{new Date(backlog.deadline).toDateString()}</td>
                            <td>{new Date(backlog.completionDate).toDateString()}</td>
                            <td>

                                <select onChange={this.select} value={Number(backlog.taskStatus.problemStatusId - 1)} id={1}>
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
                                <textarea id={8} value={backlog.description} cols={66} rows={8}
                                    onChange={this.inputText} />
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
                <h1 id="tabelLabel" >Create</h1>
                <p>This component is under development.</p>
                {contents}
            </div>
        );
    }

    // TODO: перед setState проверяй, смонтирован ли компонент !!!!!

    async getEntriesData() {
        const response = await fetch('entry/ongetentry?id=' + this.id);
        const data = await response.json();

        data.description = "";
        data.initiator = this.state.users[0];
        data.executor = this.state.users[0];
        data.taskStatus = this.state.problemStatuses[0];
        // ISO конвертится на стороне .NET
        data.startDate = this.state.date.toISOString();
        data.deadline = this.state.date.toISOString();

        if (this.mounted) this.setState({ backlog: data, loading: false });
    }

    async postEntriesData() {
        var requestBody = JSON.stringify(this.state.backlog);
        const response = await fetch('entry/onpostcreate',
            { method: "POST", headers: { 'Content-Type': "application/json;charset=utf-8" }, body: requestBody });
        const data = await response.json();
        // если Create не удался, но больше похоже на костыль
        if (data.initiator === null) {
            console.log("CREATE ABORTED");
            const data2 = this.state.backlog;
            data2.description = data.description;
            if (this.mounted) this.setState({ backlog: data2, loading: false });
        }
        else {
            if (this.mounted) this.setState({ backlog: data, loading: false });
        }
    }

    async getProblemStatus() {
        const response = await fetch('entry/ongetproblemstatuses');
        const data = await response.json();
        if (this.mounted) this.setState({ problemStatuses: data });
    }

    async getUsers() {
        const response = await fetch('entry/ongetusers');
        const data = await response.json();
        if (this.mounted) this.setState({ users: data });
    }
}