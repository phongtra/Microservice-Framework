import React from "react";

const QuestionField = props => {
  return (
    <>
      <div className="form-group">
        <label>{props.label}</label>
        <input className="form-control" value={props.value} onChange={props.onChange} />
      </div>
    </>
  );
};

export default QuestionField;
