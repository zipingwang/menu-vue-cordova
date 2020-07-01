<template lang="html">
    <i-button type="primary" :loading="busyWithSending" @click="click">
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
      default: 9, /* in seconds */
      type: Number
    }
  },
  data() {
    return {
      busyWithSending: false,
      startTime: {},
      mySendingTimer: 0
    }
  },
  methods: {
    click() {
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
