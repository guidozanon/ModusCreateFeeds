import React, {Component} from 'react'
import { feedsActionCreators } from '../store/Feeds';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom'
import '../Site.css'

class FeedsPage extends Component{
    constructor(props) {
        super(props);

        this.state = {
        };

    }
    componentDidMount() {
        this.ensureDataFetched();
    }

    ensureDataFetched() {
        this.props.requestFeeds();
      }

    render(){
        
        return(
            <div>
                <h1>Available Feeds</h1>
                <hr></hr>
                <ul className="list-group">
                    {this.props.feeds.map(feed => 
                        <li key={feed.Id} className="list-group-item" style={{cursor:'pointer'}} >
                            <Link to={'/feeds/' + feed.Id}><span className='title'>{feed.Name}</span></Link>
                            <p>{feed.Description}</p>
                            <a className='subscribe' href='javascript:void(0);' onClick={() => this.props.requestSubscribe(feed.Id, !feed.IsSubscribed)}>{!feed.IsSubscribed ? 'Subscribe' : 'Unsubscribe'}</a>
                        </li>)}
                </ul>
            </div>
        );
    }
}

export default connect(
    state => state.feeds,
    dispatch => bindActionCreators(feedsActionCreators, dispatch)
  )(FeedsPage);