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

export function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
