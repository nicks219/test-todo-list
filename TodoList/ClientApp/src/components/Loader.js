import React from 'react';

export function testSleep(ms) {

    return new Promise(resolve => setTimeout(resolve, ms));
}

export async function getProblemStatuses(component) {

    const response = await fetch('entry/ongetproblemstatuses');
    const data = await response.json();
    if (component.mounted) component.setState({ problemStatuses: data });
}

export async function getUsers(component) {

    const response = await fetch('entry/ongetusers');
    const data = await response.json();
    if (component.mounted) component.setState({ users: data });
}

export const RenderContent = ({ component }) => {

    let contents = component.state.loading
        ? <p><em>Please wait...</em></p>
        : component.renderBacklogTable(component.state.backlog);
        
    return (
        <div>
            <h1 id="tabelLabel" >{component.displayName}</h1>
            <p>This component is under development.</p>
            {contents}
        </div >
    );
}