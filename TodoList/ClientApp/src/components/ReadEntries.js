import React, { Component } from 'react';

export class ReadEntries extends Component {
    static displayName = ReadEntries.name;
    page = 0;

    constructor(props) {
        super(props);
        this.state = { backlog: [], loading: true };
    }

    componentDidMount() {
        this.getEntriesData();
    }

    back = () => {
        this.page--;
        this.getEntriesData();
    }

    forw = () => {
        this.page++;
        this.getEntriesData();
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
                    <tr key={ "button" }>
                        <th>
                            <button onClick={this.back} className="btn btn-info">&lt;BACK</button>
                        </th>
                        <th>
                            <button onClick={this.forw} className="btn btn-info">FORW&gt;</button>
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
                            <tr style={{ backgroundColor: this.checkVailidity(backlog) == true ? "white" : "red" }}>
                                <td>{backlog.title}</td>
                                <td>{backlog.initiator.name}</td>
                                <td>{backlog.executor.name}</td>
                                <td>{backlog.deadline}</td>
                                <td>{backlog.startDate}</td>
                                <td>{backlog.completionDate}</td>
                            </tr>
                            <tr>
                                <th colSpan="2" scope="row">Description</th>
                                <td colSpan="3" style={{ display: 'none' }}>{backlog.description}</td>
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
                <h1 id="tabelLabel" >Backlog</h1>
                <p>This component is under development.</p>
                {contents}
            </div>
        );
    }

    async getEntriesData() {
        const response = await fetch('entry/ongetpage?page=' + this.page);
        const data = await response.json();
        if (data != null) this.page = data[0].currentPage;
        this.setState({ backlog: data, loading: false });
    }
}