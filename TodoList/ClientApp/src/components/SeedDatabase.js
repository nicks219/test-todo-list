import React, { Component } from 'react';

export class SeedDatabase extends Component {
    static displayName = SeedDatabase.name;

    constructor(props) {
        super(props);
        this.state = { currentCount: 0, flag: 'false' };
        this.incrementCounter = this.incrementCounter.bind(this);
    }

    incrementCounter() {
        this.setState({
            currentCount: this.state.currentCount + 1
        });
        this.postSeedData();
    }

    render() {
        return (
            <div>
                <h1>Seeding counter</h1>

                <p>Press this button to allow us to seed database.</p>

                <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

                <p aria-live="polite">Seeding: <strong>{this.state.flag}</strong></p>

                <button className="btn btn-primary" onClick={this.incrementCounter}>Seed</button>
            </div>
        );
    }

    async postSeedData() {
        const response = await fetch('entry', { method: "POST" });
        const data = await response.json();
        this.setState({ flag: data.toString() });
    }
}