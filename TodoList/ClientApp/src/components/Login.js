import React, { Component } from 'react';

export class Login extends Component {

    // NB: ВНИМАНИЕ, ИСПОЛЬЗУЕТСЯ ЗАГЛУШКА ЛОГИНА
    mounted = false;

    constructor(props) {
        super(props);
        this.submit = this.submit.bind(this);
        this.url = "/account/login";
        this.state = { style: "submitStyle" };
        document.getElementById("login").style.display = "block";
    }

    componentDidMount() {
        this.mounted = true;
    }

    componentWillUnmount() {
        this.mounted = false;
    }

    submit(e) {
        e.preventDefault();
        let userName = document.getElementById("name").value;
        // NB: ЗАГЛУШКА
        userName = "Slame";
        window.fetch(this.url + "?userName=" + String(userName))
            .then(response => response.ok ? this.loginOk(response) : console.log("Login error"));
    }

    loginOk = (response) => {

        console.log(response.text());

    }

    render() {
        return (
            <div>
                <span id={this.state.style}>
                    <button type="checkbox" id="loginButton" className="btn btn-info" onClick={this.submit} >
                        LOGIN
                    </button>
                </span>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <span>
                    <input type="text" id="name" />
                </span>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <span>
                    <input type="text" id="password" />
                </span>
            </div>
        );
    }
}