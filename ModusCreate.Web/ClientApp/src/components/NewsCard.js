import React, {Component} from 'react'
import ReactMarkdownn from 'react-markdown'

export default class NewsCard extends Component{

    render(){
        return(
            <div className="card" style={{marginBottom:20}}>
                <div className="card-header">
                    <h1>{this.props.Title} <span style={{float:"right", fontSize:12}}>{this.props.CreatedOn}</span></h1>
                    <p>{this.props.FeedName} {this.props.Tags.split(" ").map(tag =>  <span key={this.props.Id + '_' + tag} className="badge badge-secondary" style={{marginRight:10}}>{tag}</span>)}</p>
                </div>
                <div className="card-body">
                    <ReactMarkdownn source={this.props.Body} />
                </div>
            </div>
        );
    }

}