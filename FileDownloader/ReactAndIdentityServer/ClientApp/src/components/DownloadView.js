import React, { Component } from "react";
import axios from "axios";
import authService from "./api-authorization/AuthorizeService";

export default class DownloadView extends Component {
  state = { token: null };
  async componentDidMount() {
    const token = await authService.getAccessToken();
    this.setState({ token });
  }
  onHandleClick = async () => {
    await axios.post("http://localhost:1200/values", {
      value:
        "https://drive.google.com/uc?export=download&id=1AKZTeum65Fqwal95lCGf3TirjNMyJz5q"
    });
  };
  render() {
    return (
      <div>
        {this.state.token ? (
          <button onClick={this.onHandleClick}>Download file</button>
        ) : (
          ""
        )}
      </div>
    );
  }
}
