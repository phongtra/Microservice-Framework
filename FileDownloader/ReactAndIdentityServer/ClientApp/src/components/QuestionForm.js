import React, { Component } from "react";
import axios from "axios";
import authService from "./api-authorization/AuthorizeService";
import FormField from "./FormField";

class QuestionForm extends Component {
  state = { title: "", content: "", authorId: "", data: [] };
  componentDidMount() {
    axios.get("/api/authors").then(res => {
      this.setState({ data: res.data });
    });
  }
  onSubmit = async e => {
    e.preventDefault();
    const token = await authService.getAccessToken();
    axios
      .post(
        "http://localhost:2000/questions-route",
        {
          title: this.state.title,
          content: this.state.content,
          authorId: parseInt(this.state.authorId)
        },
        { headers: { Authorization: `Bearer ${token}` } }
      )
      .then(res => {
        this.props.history.push(`/question-view/${this.state.authorId}`);
      });
  };

  onSelectChange = e => {
    this.setState({ authorId: e.target.value });
  };
  render() {
    return (
      <div>
        <form onSubmit={this.onSubmit}>
          <FormField
            label={"Title"}
            value={this.state.title}
            onChange={e => this.setState({ title: e.target.value })}
          />
          <FormField
            label={"Content"}
            value={this.state.content}
            onChange={e => this.setState({ content: e.target.value })}
          />
          <label>User</label>
          <select
            className="form-control"
            value={this.state.authorId}
            onChange={this.onSelectChange}
          >
            <option></option>
            {this.state.data.map((d, i) => {
              return (
                <option key={i} value={d.id}>
                  {d.name}
                </option>
              );
            })}
          </select>
          <button className="btn btn-primary">Submit</button>
        </form>
      </div>
    );
  }
}

export default QuestionForm;
