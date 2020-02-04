import React, { Component } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

import question from "../assets/question.jpg";
import DownloadView from "./DownloadView";
import SignalR from "./SignalR";

class UserView extends Component {
  state = { data: [], loading: true };
  componentDidMount() {
    axios.get("http://localhost:2000/authors-route").then(res => {
      this.setState({ data: res.data, loading: false });
    });
  }
  render() {
    if (this.state.loading) {
      return <div>Loading...</div>;
    }
    if (this.state.data.length === 0) {
      return <div>There is no users</div>;
    }

    return (
      <div className="container">
        <h1
          style={{ textAlign: "center", marginBottom: "2vh", color: "#cc121c" }}
        >
          Welcome to the Users' Page
        </h1>
        <p
          style={{
            fontSize: "2vh",
            textAlign: "center",
            margin: "0 15vh"
          }}
        >
          In this page, you will be able to view all the users who have been
          using our application. You can also click on their name to view their
          posted questions.
        </p>
        <img
          src={question}
          style={{
            height: "25vh",
            width: "15vh",
            display: "block",
            marginLeft: "auto",
            marginRight: "auto"
          }}
          alt="Display"
        />
        <h3 style={{ textAlign: "center", marginBottom: "2vh" }}>
          Click on their name to view their questions
        </h3>
        <div className="row">
          {this.state.data.map((d, i) => {
            return (
              <div className="col" key={i}>
                <div className="card">
                  <div className="card-body">
                    <h4 className="card-title">{d.name}</h4>
                    <p className="card-text">Birthplace: {d.birthplace}</p>
                    <Link
                      className="btn btn-primary"
                      to={`/question-view/${d.id}`}
                    >
                      See questions
                    </Link>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
        <DownloadView />
        <SignalR />
      </div>
    );
  }
}

export default UserView;
