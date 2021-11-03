import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Header, TableBody, Select } from './Table';
import { RenderContent, getProblemStatuses } from './Loader';

export class Update extends Component {
    displayName = Update.name;

    mounted = true;

    id = 0;

    page = 0;

    filter = 0;

    expired = "#333333";

    constructor(props) {
        super(props);

        this.state = { backlog: [], problemStatuses: [], loading: true };
        // NB: костыль, при "обновлении страницы" будет загружена первая запись, но хотя бы не "отвалится"
        if (props.location.fromReadComponent != undefined) {
            this.id = props.location.propsState;
            this.page = props.location.fromReadComponent;
        }
    }

    async componentDidMount() {
        await getProblemStatuses(this);
        await this.getEntriesData();
        this.mounted = true;
    }

    componentWillUnmount() {
        this.mounted = false;
    }

    update = () => {
        this.putEntriesData();
    }

    select = (e) => {

        const data = this.state.backlog;
        const status = Number(++e.target.value);
        data.taskStatus.problemStatusId = status;
        this.setState({ backlog: data });
    }

    checkValidity = (backlog) => {
        // TODO: сделай валидацию по дэдлайну или удали этот метод
        return false;
    }

    inputText = (e) => {

        const newText = e.target.value;
        const data = this.state.backlog;
        data.description = newText;
        this.setState({ backlog: data });
    }

    async getEntriesData() {

        const response = await fetch('entry/ongetentry?id=' + this.id);
        const data = await response.json();
        if (data.description === null) {
            console.log("Seed DB please...");
        }

        if (this.mounted) {
            this.setState({ backlog: data, loading: false });
        }
    }

    async putEntriesData() {

        var requestBody = JSON.stringify(this.state.backlog);
        const response = await fetch('entry',
            { method: "PUT", headers: { 'Content-Type': "application/json;charset=utf-8" }, body: requestBody });
        const data = await response.json();
        if (data.initiator === null) {
            console.log("UPDATE ABORTED");
            const data2 = this.state.backlog;
            data2.description = data.description;
            if (this.mounted) {
                this.setState({ backlog: data2, loading: false });
            }
        }
        else if (this.mounted) {
            this.setState({ backlog: data, loading: false });
        }
    }

    render() {

        return (
            <RenderContent component={this} />
        );
    }

    renderBacklogTable(backlog) {

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel" id="theme">
                <thead>
                    <tr key={"button"}>
                        <th>
                            <button onClick={this.update} className="btn btn-info">UPDATE</button>
                        </th>
                        <th>
                            <Link to={{
                                pathname: '/',
                                fromUpdateComponent: this.page,
                                filter: this.state.backlog.taskStatus.problemStatusId
                            }}>
                                <button className="btn btn-info">RETURN</button>
                            </Link>
                        </th>
                    </tr>
                    <tr>
                        <Header />
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    <React.Fragment key={backlog.entryId}>
                        <tr style={{ backgroundColor: this.checkValidity(backlog) === true ? "white" : this.expired }}>
                            <TableBody backlog={backlog} />
                            <td>
                                <Select select={this.select} value={backlog.taskStatus.problemStatusId} list={this.state.problemStatuses} id={1} />
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
}