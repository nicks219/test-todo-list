import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Entries } from './components/Entries';
import { SeedDatabase } from './components/SeedDatabase';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Entries} />
                <Route path='/seed-db' component={SeedDatabase} />
                <Route path='/get-entries' component={Entries} />
            </Layout>
        );
    }
}