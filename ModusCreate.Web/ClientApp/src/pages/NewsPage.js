import React, {Component} from 'react'
import { feedsActionCreators } from '../store/Feeds';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import NewsContainer from '../components/NewsContainer';

class NewsPage extends Component{
    constructor(props) {
        super(props);

        this.state = {
            searchText: ''
        };

        this.handleInputChange = this.handleInputChange.bind(this);
    }
    
    componentDidMount() {
        this.ensureDataFetched();
    }

    ensureDataFetched() {
        let w = document.querySelector("html").scrollTop;
        if (this.state.scrollTo !== w)
            this.setState({scrollTo: w});

        this.props.requestNews(this.props.newsNext, this.state.searchText);
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });
      }

    enterPressed(event) {
        var code = event.keyCode || event.which;
        if(code === 13) { //13 is the enter keycode
            this.ensureDataFetched();
        } 
    }

    onBlur(event) {
        this.ensureDataFetched();
    }

    componentDidUpdate(){
        if (this.props.loadingNews !== true){
            document.querySelector("html").scrollTop = this.state.scrollTo;
        }
    }

    render(){
        return(
            <div>
                {this.props.loadingNews === true && this.props.news.length === 0?
                    <h3>loading news...</h3> :
                    <div>
                        <h1>All News! 
                            <input type='text' 
                                onBlur={this.onBlur.bind(this)} 
                                placeholder='search by title...' 
                                value={this.state.searchText} 
                                onChange={this.handleInputChange.bind(this)}
                                onKeyPress={this.enterPressed.bind(this)}
                                name='searchText' 
                                style={{float:"right", width:300, fontSize:16, verticalAlign:"middle"}} />
                        </h1>

                        <NewsContainer newscount={this.props.newscount} news={this.props.news} ensureDataFetched={() => this.ensureDataFetched()}></NewsContainer>
                    </div>
                }
            </div>
        );
    }
}

export default connect(
    state => state.feeds,
    dispatch => bindActionCreators(feedsActionCreators, dispatch)
  )(NewsPage);