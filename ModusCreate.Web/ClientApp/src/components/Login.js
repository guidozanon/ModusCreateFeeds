import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import Modal from 'react-awesome-modal';
import { actionCreators } from '../store/User';

class Login extends Component{
    constructor(props) {
       
        super(props);

        this.state = {
            email: '',
            password:''
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

    login(){
        this.props.requestLogin(this.state.email, this.state.password);
    }

    render(){
        return(
            <Modal visible={true} width="400"  effect="fadeInDown">
            <div style={{padding:'20px'}}>
                <h1>Login</h1>
                <form>
                    <div className='form-group'>
                        <label>Email</label>
                        <input
                            name="email"
                            type="text"
                            className='form-control'
                            value={this.state.email}
                            onChange={this.handleInputChange} />
                    </div>
                    <div className='form-group'>
                        <label>Password</label>
                        <input
                            className='form-control'
                            name="password"
                            type="password"
                            value={this.state.password}
                            onChange={this.handleInputChange} />
                    </div>

                    {this.props.loginErr != null ?
                    <p className='alert alert-danger'>{this.props.loginErr}</p> : <span></span>}
                    {this.props.isloading ? 
                        <a href='javascript:void(0);' disabled >Login</a> :
                        <a href='javascript:void(0);'  onClick={ () => this.props.requestLogin(this.state.email, this.state.password) }>Login</a> }
                    
                    <button  onClick={() => this.props.closeLogin()}>Cancel</button>
                </form>
            </div>
            </Modal>
        );
    }
}


export default connect(
    state => state.user,
    dispatch => bindActionCreators(actionCreators, dispatch)
  )(Login);