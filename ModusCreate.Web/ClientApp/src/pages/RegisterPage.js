import React, {Component} from 'react'
import { actionCreators } from '../store/User';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import Login from '../components/Login';

class RegisterPage extends Component{
    constructor(props) {
        super(props);

        this.state = {
            name: '',
            email: '',
            password: ''
        };

        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });
      }

    register(){
        this.props.requestRegister(this.state.name, this.state.email, this.state.password);
    }

    render(){
        return(
            <div>
                {this.props.showLogin && this.props.user == null ? 
                    <Login></Login>
                    : null
                }
                <form>
                    <p>Enter your information to register as a new user</p>
                    <div className='form-group'>
                        <label>Name</label>
                        <input name="name" type='text' className='form-control'
                        value={this.state.name}
                        onChange={this.handleInputChange}
                        ></input>
                    </div>
                    <div className='form-group'>
                        <label>Email</label>
                        <input name="email" type='email' className='form-control'
                        value={this.state.email}
                        onChange={this.handleInputChange}></input>
                    </div>
                    <div className='form-group'>
                        <label>Password</label>
                        <input name="password" type='password' className='form-control' value={this.state.password}
                                onChange={this.handleInputChange}></input>
                    </div>
                    {this.props.registerErr != null?
                    <p className='alert alert-danger'>{this.props.registerErr}</p> : <span/>
                    }
                    <a href='javascript:void(0);' onClick={ () => this.props.requestRegister(this.state.name, this.state.email, this.state.password) }>Register</a>
                </form>
            </div>
        );
    }
}

export default connect(
    state => state.user,
    dispatch => bindActionCreators(actionCreators, dispatch)
  )(RegisterPage);