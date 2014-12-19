/** @jsx React.DOM */

var EnvList = React.createClass({

    createItem: function(item) {
      return <EnvItem data={ item } />;
    },

    render: function() {
      return <div className='row'>{ this.props.items.map(this.createItem)}</div>
    }

});
