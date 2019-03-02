import React from 'react';
import NewsCard from './NewsCard';
import '../Site.css';

export default class NewsContainer extends React.Component{

    renderEmpty(){
        return (
            <p>Seems that there are no news to show!</p>
        );
    }

    

    renderNews(){
        return(
            <div>
            
            {this.props.news.map(newsc => 
                <NewsCard key={newsc.Id} {...newsc} />
            )}

            {this.props.newscount > this.props.news.length ?
                <a className='more-news' href='javascript:void(0);' onClick={() => this.props.ensureDataFetched()}>Load more news...</a>
                : <span></span>
            }

            </div>
        );
    }

    render(){
        return(
        <div>

            {this.props.news.length > 0 ? 
            this.renderNews():
            this.renderEmpty()}    
        </div>
        )
    };
}