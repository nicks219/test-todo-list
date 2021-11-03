import React from 'react';

export const Select = ({ select, value, list, id }) => {

    return (
        <select onChange={select}
            value={Number(value - 1)} id={id}>
            {list.map((a, i) =>
                <option value={i} key={i.toString() + 'i'}>
                    {id === 0 ? a.name : a.problemStatusName}
                </option>
            )}
        </select>
    );
}

export const Header = () => {

    return (
        <>
            <th>Title</th>
            <th>Initiator</th>
            <th>Executor</th>
            <th>Start Date</th>
            <th>Deadline</th>
            <th>Completion Date</th>
        </>
    );
}

export const TableBody = ({ backlog }) => {

    return (
        <>
            <td>{backlog.title}</td>
            <td>{backlog.initiator.name}</td>
            <td>{backlog.executor.name}</td>
            <td>{new Date(backlog.startDate).toDateString()}</td>
            <td>{new Date(backlog.deadline).toDateString()}</td>
            <td>{new Date(backlog.completionDate).toDateString()}</td>
        </>
    );
}

export function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
