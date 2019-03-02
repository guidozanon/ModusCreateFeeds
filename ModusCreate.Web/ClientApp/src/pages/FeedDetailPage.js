import React from 'react';
import NewsContainer from '../components/NewsContainer';
import { feedsActionCreators } from '../store/Feeds';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

class FeedDetailPage extends React.Component{


    componentDidMount(){
        const { id } = this.props.match.params;
        
        this.props.requestFeed(id);

        this.props.requestFeedNews(id);
    }

    render(){
        
        return(
            <div>
            {this.props.currentFeed.loading ?
                <span>loading...</span> :
            
                <div>
                <h1>{this.props.currentFeed.feed.Name}</h1>
                <p>{this.props.currentFeed.feed.Description}</p>
                <hr></hr>
                <NewsContainer newscount={this.props.currentFeed.newscount} news={this.props.currentFeed.news} ensureDataFetched={() => this.props.requestFeedNews(this.props.currentFeed.feed.Id, this.props.currentFeed.nextLink)}></NewsContainer>
                </div>
            }
            </div>
        );
    }
}

export default connect(
    state => state.feeds,
    dispatch => bindActionCreators(feedsActionCreators, dispatch)
  )(FeedDetailPage);