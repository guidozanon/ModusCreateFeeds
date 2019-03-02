import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Feeds';

class CategoryWidget extends React.Component{
    componentDidMount() {
        this.ensureDataFetched();
    }

    ensureDataFetched() {
        this.props.requestCategories();
      }

    render(){
        return(
            <div>
                <ul>
                    {this.state.cateogies.map(category => <li>{category.name}</li>)}
                </ul>
            </div>
        );
    }
}

export default connect(
    state => state.weatherForecasts,
    dispatch => bindActionCreators(actionCreators, dispatch)
  )(CategoryWidget);