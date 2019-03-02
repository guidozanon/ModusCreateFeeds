import React, {Component} from 'react';
import { connect } from 'react-redux';
import Login from '../components/Login';
import { actionCreators } from '../store/User';
import { feedsActionCreators } from '../store/Feeds';
import { bindActionCreators } from 'redux';
import NewsContainer from '../components/NewsContainer';
import AuthService from '../services/AuthService';

class HomePage extends Component{
  
  ensureNewsDataFetched() {
    this.props.requestMyNews(this.props.feeds.myfeeds.nextLink);
  }

  componentDidUpdate(){
    if (this.props.feeds.myfeeds.news.length === 0 && this.props.feeds.myfeeds.loading === false && AuthService.isAuthenticated() && this.props.feeds.myfeeds.newscount == undefined){
      this.ensureNewsDataFetched();
    }
  }

  componentDidMount(){
    if (this.props.feeds.myfeeds.news.length === 0 && this.props.feeds.myfeeds.loading === false && AuthService.isAuthenticated() && this.props.feeds.myfeeds.newscount == undefined){
      this.ensureNewsDataFetched();
    }
  }

  renderNews(){
    return(
      <div>
        {this.props.feeds.myfeeds.loading === true && this.props.feeds.myfeeds.news.length === 0?
            <h3>loading news...</h3> :
            <div>
            <h1>Your Subscription News!</h1>
            <NewsContainer 
              newscount={this.props.feeds.myfeeds.newscount} 
              news={this.props.feeds.myfeeds.news} 
              ensureDataFetched={() => this.ensureNewsDataFetched()}
              noNewsMessage='Seems that there are no news to show. Go to the Feeds Page and make sure to subscribe at least to one Feed to get News on your NewsFeed!'
            ></NewsContainer>
            </div>
        }
    </div>
    );
  }

renderAnonimousHome(){
  return (
    <div>
      <h1>Hello, Geeta!</h1>
      <p>Welcome to <b>Guido Zanon</b> NewsFeed Test Project.</p>
      <p>My choosen Tech Stack:</p>
      <ul>
        <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
        <li><a href='https://facebook.github.io/react/'>React</a> and <a href='https://redux.js.org/'>Redux</a> for client-side code</li>
        <li><a href='https://www.odata.org/'>OData</a> Web Api for <b>Feeds / Categories / News</b></li>
        <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
        <li><a href='https://automapper.org/'>Automapper</a> for projecting DB classes into UI Models</li>
        <li><a href='https://jwt.io/'>Jwt Tokens</a> and <a href='https://docs.microsoft.com/en-us/aspnet/identity/overview/'>ASP.NET Identity</a> for authentication / authorization.</li>
      </ul>
      <p>What you will find on this Test Project:</p>
      <ul>
        <li><strong>Feed Categories</strong>: You will be able to filter and navigate Feeds or News by Categories</li>
        <li><strong>News Feeds</strong>: Navigate and subscribe to NewsFeeds of your interest </li>
        <li><strong>News</strong>:Read News from your subscribed Feeds, search by <b>Tags / title / date / content</b></li>
      </ul>
      <p>Whats missing or needs improvements:</p>
      <ul>
        <li><strong>Local cache</strong>: Better cache strategy</li>
        <li><strong>Refresh Token</strong>: Refresh jwt token when expired</li>
        <li><strong>Form validations</strong>: login / register</li>
      </ul>
      <p>Considerations:</p>
      <ul>
        <li>The news has all the same title and content.</li>
      </ul>
      <p><b>Let's Start!</b> You need to <a href='/register'>Register</a> or <a href='javascript:void(0);' onClick={() =>this.props.login()}>Login</a> if you already created a user</p>
      </div>
  );
}

render(){
  
  return(
    <div>
        {this.props.user.showLogin && this.props.user.user == null ? 
           <Login></Login>
          : null
        }

      {!AuthService.isAuthenticated() ? 
      this.renderAnonimousHome() :
      this.renderNews()}
      
    </div>
  );
  }
}

export default connect(
  state => state,
  dispatch => bindActionCreators({...actionCreators, ...feedsActionCreators}, dispatch)
)(HomePage);