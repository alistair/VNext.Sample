/** @jsx React.DOM */

var EnvItem = React.createClass({

  getInitialState: function() {
    return {
        isOn:    this.props.data.isOn,
        startAt: this.props.data.startAt,
        stopAt:  this.props.data.stopAt
    };
  },

  isOn: function() {
    return this.state.isOn
        ? "running"
        : "paused";
  },

  toggleOn: function() {

    console.log( this.props )

    var action = this.state.isOn ? EnvActions.end : EnvActions.start;
    this.setState({ isOn: !this.state.isOn }, action.bind( null, this.props.data.name ))
  },

  render: function() {
    return (
      <div className={ 'environment-switch col-sm-6 col-lg-6 col-md-6  ' + this.isOn() }>
        <div className='data-area'>
          <label>{ this.props.data.name }</label>
          <div className='cog '></div>
          <div className='start-stop-times'>
            <div className='starts-at'>
                { this.state.startAt }
            </div>
            <div className='stops-at'>
                { this.state.stopAt }
            </div>
          </div>
          <div className='switch-container' onClick={ this.toggleOn }>
            <Switch value={ this.state.isOn } />
          </div>
        </div>
      </div>
    );
  }
});

