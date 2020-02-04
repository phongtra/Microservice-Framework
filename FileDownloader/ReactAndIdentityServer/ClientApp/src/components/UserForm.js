import React, { Component } from "react";
import axios from "axios";

import FormField from "./FormField";

class UserForm extends Component {
  state = { name: "", birthplace: "" };
  onSubmit = e => {
    e.preventDefault();
    axios
      .post("http://localhost:2000/authors-route", {
        name: this.state.name,
        birthplace: this.state.birthplace
      })
      .then(res => {
        this.props.history.push("/");
      });
  };
  render() {
    return (
      <div>
        <form onSubmit={this.onSubmit}>
          <FormField
            label={"Author"}
            value={this.state.name}
            onChange={e => this.setState({ name: e.target.value })}
          />
          <FormField
            label={"Birthplace"}
            value={this.state.birthplace}
            onChange={e => this.setState({ birthplace: e.target.value })}
          />
          <button>Submit</button>
        </form>
      </div>
    );
  }
}

export default UserForm;
