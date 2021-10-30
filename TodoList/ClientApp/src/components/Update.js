import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Update extends Component {
    static displayName = Update.name;
    //page = 0;
    id = 0;

    constructor(props) {
        super(props);
        this.state = { backlog: [], loading: true };
        this.id = props.location.propsState;
        console.log(this.id);
    }

    componentDidMount() {
        this.getEntriesData();
    }

    update = () => {
        this.putEntriesData();
    }

    expired = {
        backgroundColor: "#FF0000"
    }

    checkVailidity = (backlog) => {
        //backlog.deadline
        //backlog.taskStatus.problemStatusName
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

                    <React.Fragment key={backlog.entryId}>
                        <tr style={{ backgroundColor: this.checkVailidity(backlog) == true ? "white" : "red" }}>
                            {/*<td><Link to='/seed-db'>{backlog.title}</Link></td>*/}
                            <td>{backlog.title}</td>
                            <td>{backlog.initiator.name}</td>
                            <td>{backlog.executor.name}</td>
                            <td>{backlog.deadline}</td>
                            <td>{backlog.startDate}</td>
                            <td>{backlog.completionDate}</td>
                        </tr>
                        <tr>
                            <th colSpan="2" scope="row">Description</th>
                            <td colSpan="3" style={{ display: '' }}>
                                <textarea id={backlog.entryId} value={backlog.description} cols={66} rows={8} onChange={this.inputText} />
                            </td>
                        </tr>
                    </React.Fragment>

                </tbody>
            </table>
        );
    }

    inputText = (e) => {
        var id = Number(e.target.id);

        const newText = e.target.value;
        const data = [{ description: newText, initiator: { name: '' }, executor: { name: '' }, entryId: '1' }];
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
        //if (data != null) this.page = data[0].currentPage;
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
        //if (data != null) this.page = data[0].currentPage;
        this.setState({ backlog: data, loading: false });
        //this.id = data;
    }
}