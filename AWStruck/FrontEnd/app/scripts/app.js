/** @jsx React.DOM */

var React = window.React = require('react'),
    Timer = require("./ui/Timer"),
    EnvList = require("./ui/EnvList"),
    mountNode = document.getElementById("app");


var dummyData = [
    { name: "Environment 1", id: "Env1", isOn: true, startAt: "07:00", stopAt: "19:00" },
    { name: "Environment 2", id: "Env2", isOn: false, startAt: "07:00", stopAt: "20:00" }
]

var App = React.createClass({
  getInitialState: function() {
    return { items: dummyData };
  },
  render: function() {
    return (
      <div>
        <h3>AWS Environments</h3>
        <div className='container-fluid' >
          <EnvList items={this.state.items} />
        </div>
      </div>
    );
  }
});

//<Timer />

React.renderComponent(<App />, mountNode);

