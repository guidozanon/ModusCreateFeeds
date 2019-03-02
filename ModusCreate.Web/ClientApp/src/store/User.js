import AuthService from '../services/AuthService';
import history from '../services/History';

const requestLogin = 'REQUEST_LOGIN';
const receiveLogin = 'RECEIVE_LOGIN';
const receiveLoginError= 'RECEIVE_LOGIN_ERROR';
const requestRegister = 'REQUEST_REGISTER';
const receiveRegister = 'RECEIVE_REGISTER';
const registerError = 'REGISTER_ERROR';

const logoff = 'LOGOFF';
const login = 'LOGIN';
const closeLogin = 'CLOSELOGIN';

const initialState = { user: AuthService.getUser(), isLoading: false, showLogin: false };

export const actionCreators = {
  requestLogin: (email, password) => async (dispatch, getState) => {
    
    dispatch({ type: requestLogin, email, password });
    
    try{
      await AuthService.login(email, password);

      const user = AuthService.getUser();
      dispatch({ type: receiveLogin, user });

      history.push('/');

    }catch(error){
      dispatch({ type: receiveLoginError, error });
    }
  },

  requestRegister: (name, email, password) => async (dispatch, getState) => {
    dispatch({ type: requestRegister, email, password });

    try{
      debugger;
      await AuthService.register(name, email, password);

      const user = AuthService.getUser();
      dispatch({ type: receiveRegister, user });

      history.push('/');
    }catch(error){
      dispatch({ type: registerError,  error});
    }
  },

  logoff: () => async(dispatch) => {

    AuthService.logoff();

    dispatch({ type: logoff});
  },
  login: () => async(dispatch) => {
    dispatch({ type: login});
  },
  closeLogin: () => async(dispatch) => {
    dispatch({ type: closeLogin});
  },
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === logoff) {
    return {
      ...state,
      user: null,
      isLoading: false  
    };
  }

  if (action.type === login) {
    return {
      ...state,
      isLoading: false,
      user: action.user,
      showLogin: true
    };
  }

  if (action.type === closeLogin) {
    return {
      ...state,
      isLoading: false,
      showLogin: false,
      loginErr: null
    };
  }

  if (action.type === requestLogin) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveLogin) {
    return {
      ...state,
      isLoading: false,
      showLogin: false
    };
  }

  if (action.type === receiveLoginError) {
    return {
      ...state,
      isLoading: false,
      showLogin: true,
      loginErr: action.error
    };
  }
  
  
  if (action.type === requestRegister) {
    return {
      ...state,
      isLoading: true
    };
  }
  if (action.type === receiveRegister) {
    return {
      ...state,
      user: action.user,
      isLoading: false
    };
  }
  if (action.type === registerError) {
    return {
      ...state,
      user: null,
      registerErr: action.error
    };
  }
  

  return state;
};
