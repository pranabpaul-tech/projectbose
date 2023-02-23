import React,{Component} from 'react';
import {NavLink} from 'react-router-dom';
import {Navbar,Nav} from 'react-bootstrap';


export class Navigation extends Component{

    render(){
        return(
            <Navbar bg="dark" expand="lg">
                <Navbar.Toggle aria-controls="basic-navbar-nav"/>
                <Navbar.Collapse id="basic-navbar-nav">
                <Nav>
                <NavLink className="d-inline p-2 bg-dark text-white" to="/">
                    Home
                </NavLink>
                <NavLink className="d-inline p-2 bg-dark text-white" to="/levels">
                    Levels
                </NavLink>
                <NavLink className="d-inline p-2 bg-dark text-white" to="/leveldetails">
                    LevelDetails
                </NavLink>
                <NavLink className="d-inline p-2 bg-dark text-white" to="/resourcedetails">
                    ResourceDetails
                </NavLink>
                </Nav>
                </Navbar.Collapse>
            </Navbar>
        )
    }
}