import React, { Component } from 'react';

export class Entries extends Component {
    static displayName = Entries.name;

    constructor(props) {
        super(props);
        this.state = { backlog: [], loading: true };
    }

    componentDidMount() {
        this.getEntriesData();
    }

    static renderBacklogTable(backlog) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
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
                        <><tr key={backlog.entryId}>
                            <td>{backlog.title}</td>
                            <td>{backlog.initiator.name}</td>
                            <td>{backlog.executor.name}</td>
                            <td>{backlog.deadline}</td>
                            <td>{backlog.startDate}</td>
                            <td>{backlog.completionDate}</td>
                        </tr>
                            <tr>
                                <th colSpan="2" scope="row">Description</th>
                                <td colSpan="3">{backlog.description}</td>
                            </tr></>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Please wait...</em></p>
            : Entries.renderBacklogTable(this.state.backlog);

        return (
            <div>
                <h1 id="tabelLabel" >Backlog</h1>
                <p>This component is under development.</p>
                {contents}
            </div>
        );
    }

    async getEntriesData() {
        const response = await fetch('entry');
        const data = await response.json();
        this.setState({ backlog: data, loading: false });
    }
}