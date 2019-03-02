import React from 'react'
import AuthService from '../services/AuthService';
import { Redirect, Route } from 'react-router-dom'

const AuthRoute = ({ component: Component, ...rest }) => {

  // Add your own authentication on the below line.
  const isLoggedIn = AuthService.isAuthenticated()

  return (
    <Route
      {...rest}
      render={props =>
        isLoggedIn ? (
          <Component {...props} />
        ) : (
          <Redirect to={{ pathname: '/', state: { from: props.location } }} />
        )
      }
    />
  )
}

export default AuthRoute