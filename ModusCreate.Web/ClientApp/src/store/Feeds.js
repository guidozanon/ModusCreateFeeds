import AuthService from "../services/AuthService";
import { actionCreators } from "./User";

const requestCategories = 'REQUEST_CATEGORIES';
const receiveCategories = 'RECEIVE_CATEGORIES';
const requestFeeds = 'REQUEST_FEEDS';
const receiveFeeds = 'RECEIVE_FEEDS';


const requestFeed = 'REQUEST_FEED';
const receiveFeed = 'RECEIVE_FEED';

const requestNews = 'REQUEST_NEWS';
const receiveNews = 'RECEIVE_NEWS';

const requestMyNews = 'REQUEST_MYNEWS';
const receiveMyNews = 'RECEIVE_MYNEWS';

const requestSubscribe = 'REQUEST_SUBSCRIBE';
const receiveSubscribe = 'RECEIVE_SUBSCRIBE';

const requestFeedNews = 'REQUEST_FEEDNEWS';
const receiveFeedNews = 'RECEIVE_FEEDNEWS';

const initialState = { categories: [], feeds: [], news:[], newscount:0, newsNext:'', loadingNews:false,  currentFeed: {loading:true}, myfeeds:{news:[], loading:false} };


export function getHeader(){
    let headers = {"Content-Type": "application/json"};
    headers["Authorization"] = `bearer ${AuthService.getUser().token.jwt}`;
    return headers;
}

export const feedsActionCreators = {
  requestCategories: () => async (dispatch, getState) => {
    
    dispatch({ type: requestCategories });

    const url = `api/categories`;
    const response = await fetch(url,{ headers: getHeader() });
    const categories = await response.json();

    dispatch({ type: receiveCategories, categories });
  },

  
  requestFeeds: () => async (dispatch, getState) => {
    dispatch({ type: requestFeeds });

    const response = await fetch(`odata/feeds`, { headers: getHeader() });
    if (response.ok){
      const feeds = (await response.json()).value;

      dispatch({ type: receiveFeeds, feeds });
    }else{
      console.error(response.statusText);
    }
  },

  requestFeed: (id) => async (dispatch, getState) => {
    dispatch({ type: requestFeed });

    const { feeds } = getState();
    
    let feed = feeds.feeds.find((f)=>{return f.Id === id ;});
    
    if(feed == null){
      const url = `odata/feeds(${id})`;
      const response = await fetch(url,{ headers: getHeader() });
      feed = await response.json();
    }
    
    dispatch({ type: receiveFeed, feed });
  },

  requestFeedNews: (id, nextLink) => async (dispatch, getState) => {
    dispatch({ type: requestFeedNews, id });

      let url = `odata/feeds(${id})/News?$count=true`;

      if (nextLink != null && nextLink.indexOf(id) >= 0){
        url =  nextLink;
      }

      const response = await fetch(url,{ headers: getHeader() });
      let news = await response.json();
    
      const feednews = {
          newscount: news['@odata.count'],
          nextLink: news['@odata.nextLink'],
          news: news.value,
          feedId: id
      };
    dispatch({ type: receiveFeedNews, feednews });
  },

  requestMyNews: (nextUrl, searchText) => async (dispatch, getState) => {
    dispatch({ type: requestMyNews, nextUrl });
    
    let url = `odata/mynews?$count=true&$orderby=CreatedOn%20desc`;
    if(searchText != null){
      url +=`&$filter=contains(Title, '${searchText}')`;
    }

    if (nextUrl !== '' && nextUrl !== undefined && (searchText == null || nextUrl.indexOf(encodeURI(searchText))>=0)){
      url = nextUrl;
    }

    const response = await fetch(url,{ headers: getHeader() });
    if (response.ok){
      const json = await response.json();
      const myfeeds = {
        news: json.value,
        newscount: json['@odata.count'],
        nextLink: json['@odata.nextLink']
      };

      dispatch({ type: receiveMyNews, myfeeds});
    }else{
      console.log(response.statusText);
    }
  },

  requestNews: (nextUrl, searchText) => async (dispatch, getState) => {
    dispatch({ type: requestNews, nextUrl });
    
    let url = `odata/news?$count=true&$orderby=CreatedOn%20desc`;
    if(searchText != null){
      url +=`&$filter=contains(Title, '${searchText}')`;
    }

    if (nextUrl !== '' && nextUrl !== undefined && nextUrl.indexOf(encodeURI(searchText))>=0)
      url = nextUrl;

    const response = await fetch(url,{ headers: getHeader() });
    if (response.ok){
      const json = await response.json();
      const news = json.value;
      const newscount = json['@odata.count'];
      const newsNext = json['@odata.nextLink'];
      dispatch({ type: receiveNews, news, newscount,  newsNext, searchText });
    }else{
      console.log(response.statusText);
    }
  },

  requestSubscribe: (id, subscribe) =>async (dispatch, getState) => {
    dispatch({ type: requestSubscribe, id, subscribe });

    const response = await fetch(`odata/feeds(${id})`,{
        method: subscribe ? 'post' : 'delete',
        headers: getHeader(),
      });

      if (response.ok){
        const feedId = id;
        const result = subscribe;
        dispatch({ type: receiveSubscribe, feedId, result});
      }else{
        console.error(response.statusText);
      }
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestCategories) {
    return {
      ...state,
    };
  }

  if (action.type === receiveCategories) {
    return {
      ...state,
      categories: action.categories,
    };
  }

  if (action.type === receiveFeeds) {
    return {
      ...state,
      feeds: action.feeds,
    };
  }

  if (action.type === receiveNews) {
    if (state.searchText !== action.searchText)
      state.news = [];

    return {
      ...state,
      news: state.news.concat(action.news),
      newscount: action.newscount,
      newsNext: action.newsNext,
      searchText: action.searchText,
      loadingNews: false
    };
  }

  if (action.type === requestNews) {
    return {
      ...state,
      loadingNews: true
    };
  }
  
  if (action.type === requestFeed) {
    return {
      ...state,
      currentFeed: {
        loading:true,
        feed: null,
        news:[]
      }
    };
  }

  if (action.type === receiveFeed) {
    return {
      ...state,
      currentFeed: {
        loading:false,
        feed: action.feed,
        news:[]
      }
    };
  }

  if (action.type === receiveFeedNews) {
    let news ={news: state.currentFeed.news.concat(action.feednews.news)};
    let c = {...state.currentFeed, ...action.feednews, ...news};

    return {
      ...state,
      currentFeed: c
    };
  }
  
  
  if (action.type === requestMyNews) {
    return {
      ...state,
      myfeeds: {...state.myfeeds, ...{loading:true}}
    };
  }

  if (action.type === receiveMyNews) {
    action.myfeeds.news = state.myfeeds.news.concat(action.myfeeds.news);

    return {
      ...state,
      myfeeds: {...action.myfeeds, ...{loading:false}}
    };
  }
  

  if (action.type === receiveSubscribe){
    let f = state.feeds.find(x => x.Id === action.feedId);
    if (f != null)
      f.IsSubscribed = action.result;
    return {
      ...state,
      myfeeds: initialState.myfeeds,
      lastSubscription: `${action.feedId}_${action.result}`
      };
  }

  return state;
};
