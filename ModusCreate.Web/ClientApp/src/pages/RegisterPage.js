import React, {Component} from 'react'
import { actionCreators } from '../store/User';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

class RegisterPage extends Component{
    constructor(props) {
        super(props);

        this.state = {
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
                <button onClick={()=>this.register()}>Register</button>
            </form>
        );
    }
}

export default connect(
    state => state.user,
    dispatch => bindActionCreators(actionCreators, dispatch)
  )(RegisterPage);