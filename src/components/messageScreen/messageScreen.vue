<style lang="stylus" scoped>
* {
  box-sizing: border-box;
}

/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: wrap;
}

/* Create four equal columns */
.column {
  flex: 25%;
  padding: 20px;
}

.billcontainer {
  display: flex;
  flex-wrap: nowrap;
  margin: 50px auto
}

.billcontent {
  flex: 80%
}

.columnpadding {
  flex : 10%
}

.ratingsWrapper
  position: absolute
  top: 174px
  bottom: 0
  left: 0
  width: 100%
  overflow: hidden

</style>

<template lang="html">

</template>

<script>
import axios from 'axios'
import star from 'components/star/star'
import BScroll from 'better-scroll'
import sendButton from 'components/common/sendButton/sendButton'

export default {
  components: {
    sendButton
  },
  props: {
    data: {},
    ml: {},
    seller: {}
  },
  data() {
    return {
    }
  },
  computed: {
  },
  mounted() {
    console.log(this.data.options.msg)
    // alert(this.ml[this.data.options.msg])
    console.log(this.ml[this.data.options.msg.content])
    switch (this.data.options.msg.type) {
      case 'info':
        this.$Modal.info({
          title: '',
          content: this.ml[this.data.options.msg.content],
          okText: this.ml.ok,
          onOk: () => { window.location.replace(this.data.options.baseUrl) }
        });
        break;
      case 'warning':
        this.$Modal.warning({
          title: '',
          content: this.ml[this.data.options.msg.content],
          okText: this.ml.ok,
          onOk: () => { window.location.replace(this.data.options.baseUrl) }
        });
        break;
      case 'error':
        this.$Modal.error({
          title: '',
          content: this.ml[this.data.options.msg.content],
          okText: this.ml.ok,
          onOk: () => { window.location.replace(this.data.options.baseUrl) }
        });
        break;
      case 'success':
        this.$Modal.success({
          title: '',
          content: this.ml[this.data.options.msg.content],
          okText: this.ml.ok,
          onOk: () => { window.location.replace(this.data.options.baseUrl) }
        });
        break;
    }
  },
  watch: {
  },
  methods: {
  }
}

</script>
