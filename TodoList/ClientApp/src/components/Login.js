import React, { Component } from 'react';

export class Login extends Component {

    // NB: ВНИМАНИЕ, ИСПОЛЬЗУЕТСЯ ЗАГЛУШКА ЛОГИНА
    mounted = false;

    constructor(props) {
        super(props);
        this.submit = this.login.bind(this);
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

    async login() {
        let userName = document.getElementById("name").value;
        // userName = "Slame";
        await fetch('login/login?userName=' + String(userName))
            .then(response => response.ok ? console.log("Login completed") : console.log("Login error"));
    }

    async logout() {
        await fetch('login/logout/')
            .then(response => response.ok ? console.log("Logout completed") : console.log("Logout error"));
    }

    loginOk = (response) => {

        console.log(response.text());
    }

    render() {
        return (
            <div>
                <span id={this.state.style}>
                    <button type="checkbox" id="loginButton" className="btn btn-info" onClick={this.login} >
                        LOGIN
                    </button>
                </span>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <span>
                    <input type="text" id="name" placeholder="enter John"/>
                </span>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <span>
                    <input type="text" id="password" />
                </span>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <span id={this.state.style}>
                    <button type="checkbox" id="loginButton" className="btn btn-info" onClick={this.logout} >
                        LOGOUT
                    </button>
                </span>
            </div>
        );
    }
}