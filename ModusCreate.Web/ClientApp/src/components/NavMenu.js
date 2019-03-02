import React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { actionCreators } from '../store/User';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import AuthService from '../services/AuthService';

class NavMenu extends React.Component {
  constructor (props) {
    super(props);

    this.toggle = this.toggle.bind(this);
    this.state = {
      isOpen: false
    };
  }
  toggle () {
    this.setState({
      isOpen: !this.state.isOpen
    });
  }
  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light >
          <Container>
            <NavbarBrand tag={Link} to="/">Modus Create - NewsFeed Test</NavbarBrand>
            <NavbarToggler onClick={this.toggle} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
            
              {AuthService.isAuthenticated() ?
                  <ul className="navbar-nav flex-grow">
                    <NavItem>
                      <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                    </NavItem>
                    <NavItem>
                      <NavLink tag={Link} className="text-dark" to="/feeds">Feeds</NavLink>
                    </NavItem>
                    <NavItem>
                      <NavLink tag={Link} className="text-dark" to="/news">All News</NavLink>
                    </NavItem>
                    <NavItem>
                      <NavLink tag={Link} className="text-dark" onClick={() => this.props.logoff()} to="/">Logoff</NavLink>
                    </NavItem>
                    <NavItem>
                      <img src={AuthService.getUser().Avatar} style={{width:40, height:40}} title={AuthService.getUser().given_name} alt={AuthService.getUser().given_name} />
                    </NavItem>
                    </ul>
                :
                  <div>
                    <a href='javascript:void(0);' onClick={ () => this.props.login() }>Login</a>
                  </div>
                }
              
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}

export default connect(
  state => state.user,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(NavMenu);