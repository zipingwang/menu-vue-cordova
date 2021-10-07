<template lang="html">
    <i-button type="primary" :disabled="disabled" :loading="busyWithSending" @click="click">
        <span v-if="!busyWithSending">{{this.text}}</span>
        <span v-else>{{this.sendingText}}...</span>
    </i-button>
</template>

<script>

export default {
  props: {
    ml: {},
    text: '',
    sendingText: {
      default: '',
      type: String
    },
    failedTitle: '',
    failedText: '',
    timeout: { /* if want to set default value, need to assign object */
      default: 15, /* in seconds */
      type: Number
    },
    disabled: {
      default: false,
      type: Boolean
    }
  },
  data() {
    return {
      busyWithSending: false,
      startTime: {},
      mySendingTimer: 0,
      prevClick: new Date()
    }
  },
  methods: {
    click() {
      var preClickTemp = new Date(this.prevClick)
      this.prevClick = new Date()
      if (preClickTemp.setMilliseconds(preClickTemp.getMilliseconds() + 300) > new Date()) {
        console.log('skip')
        return
      }
      console.log('click button')
      this.$emit('click')
    },
    start() {
      this.busyWithSending = true
      this.startTime = new Date()
      if (this.mySendingTimer) { /* prevent double click, clear previous one */
        clearInterval(this.mySendingTimer)
      }
      this.mySendingTimer = setInterval(this.checkSending, 1000)
    },
    stop() {
      this.busyWithSending = false
      clearInterval(this.mySendingTimer)
      this.startTime = null
    },
    checkSending() {
      if (this.busyWithSending === false) {
        clearInterval(this.mySendingTimer)
        console.log('return')
        return
      }

      if (this.startTime.setSeconds(this.startTime.getSeconds() + this.timeout) < new Date()) {
        this.busyWithSending = false
        clearInterval(this.mySendingTimer)
        // this.showWaiting = false
        this.$Modal.success({
          title: this.failedTitle,
          content: this.failedText,
          okText: this.ml.ok
        });
      } else {
        this.startTime.setSeconds(this.startTime.getSeconds() - this.timeout) /* set to its original time */
      }
    }
  }
}

</script>

<style lang="stylus" scoped>
</style>
