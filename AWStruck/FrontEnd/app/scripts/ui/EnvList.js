/** @jsx React.DOM */
var React = require('react'),
  EnvItem = require("./EnvItem");

var EnvList = React.createClass({

    createItem: function(item) {
      return <EnvItem data={ item } />;
    },

    render: function() {
      return <div className='row'>{ this.props.items.map(this.createItem)}</div>
    }

});

module.exports = EnvList;
