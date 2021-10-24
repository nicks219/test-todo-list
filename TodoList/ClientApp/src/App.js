import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { ReadEntries } from './components/ReadEntries';
import { SeedDatabase } from './components/SeedDatabase';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={ReadEntries} />
                <Route path='/seed-db' component={SeedDatabase} />
                <Route path='/get-entries' component={ReadEntries} />
            </Layout>
        );
    }
}