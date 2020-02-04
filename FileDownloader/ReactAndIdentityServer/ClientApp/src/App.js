import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { FetchData } from "./components/FetchData";
import { Counter } from "./components/Counter";
import UserView from "./components/UserView";
import UserForm from "./components/UserForm";
import QuestionForm from "./components/QuestionForm";
import QuestionView from "./components/QuestionView";
import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import { ApplicationPaths } from "./components/api-authorization/ApiAuthorizationConstants";

import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={UserView} />
        <Route path="/user-form" component={UserForm} />
        <Route path="/question-form" component={QuestionForm} />
        <Route path="/question-view/:id" component={QuestionView} />
        <Route path="/counter" component={Counter} />
        <AuthorizeRoute path="/fetch-data" component={FetchData} />
        <Route
          path={ApplicationPaths.ApiAuthorizationPrefix}
          component={ApiAuthorizationRoutes}
        />
      </Layout>
    );
  }
}
