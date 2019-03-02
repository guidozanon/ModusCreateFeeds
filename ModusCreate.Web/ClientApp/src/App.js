import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import HomePage from './pages/HomePage';
import FeedsPage from './pages/FeedsPage';
import NewsPage from './pages/NewsPage';
import RegisterPage from './pages/RegisterPage';
import FeedDetailPage from './pages/FeedDetailPage';
import AuthRoute from './components/AuthRoute'
export default () => (
  <Layout>
    <Route exact path='/' component={HomePage} />
    <AuthRoute path='/news' component={NewsPage} />
    <AuthRoute path='/feeds' component={FeedsPage} exact strict/>
    <AuthRoute path='/feeds/:id' component={FeedDetailPage} exact strict />
    <Route path='/register' component={RegisterPage} />
  </Layout>
);
