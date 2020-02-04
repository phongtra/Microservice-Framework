import React, { Component } from "react";
import axios from "axios";
import authService from "./api-authorization/AuthorizeService";

class QuestionView extends Component {
  state = { data: [], author: {}, loading: true };
  async componentDidMount() {
    const token = await authService.getAccessToken();
    try {
      console.log("run");
      const [res1, res2] = await Promise.all([
        token
          ? axios.get(
              `http://localhost:2000/questions-route/author/${this.props.match.params.id}`,
              {
                headers: { Authorization: `Bearer ${token}` }
              }
            )
          : axios.get(
              `http://localhost:2000/questions-route/author/titles/${this.props.match.params.id}`
            ),
        axios.get(
          `http://localhost:2000/authors-route/${this.props.match.params.id}`
        )
      ]);
      this.setState({ data: res1.data, loading: false });
      this.setState({ author: res2.data });
    } catch (e) {
      this.setState({ data: null, loading: false });
    }
  }

  render() {
    if (this.state.loading) {
      return <div>Loading...</div>;
    }
    if (!this.state.author) {
      return <h1 className="display-1">Invalid author id</h1>;
    }
    if (!this.state.data) {
      return (
        <>
          <h3>There is some error</h3>
        </>
      );
    }
    if (this.state.data.length === 0) {
      return (
        <h1 className="display-1">{this.state.author.name} has no questions</h1>
      );
    }
    const {
      author: { name, birthplace },
      data
    } = this.state;
    return (
      <div className="container-fluid">
        <h1>This is user {name}</h1>
        <h1 className="display-4">
          {name} is from {birthplace}
        </h1>
        <h1>Here are {name}'s questions</h1>
        <div className="row">
          {data.map((d, i) => {
            return (
              <div className="col" style={{ marginLeft: "2vh" }} key={i}>
                <h4>{d.title ? d.title : d}</h4>
                <p>{d.content ? d.content : ""}</p>
              </div>
            );
          })}
        </div>
      </div>
    );
  }
}

export default QuestionView;
