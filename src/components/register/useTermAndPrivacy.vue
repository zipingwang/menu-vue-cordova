<template>
  <span>{{part1}}<a @click="clickUseTerm()">{{useTerm}}</a>{{part2}}<a @click="clickPrivacyPolicy()">{{privacyPolicy}}</a>{{part3}}</span>
</template>

<script>
export default {
  props: {
    ml: {}
  },
  computed: {
    txtElements() {
      if (this.ml.usetermandprivacypolicy != null) {
        return this.userTermpText.split('{|}')
      } else {
        return 'part1 {|} useterm {|} part2 {|} privacy polity {|} part3'.split('{|}')
      }
    },
    useTerm() {
      return this.txtElements[1]
    },
    privacyPolicy() {
      return this.txtElements[3]
    },
    part1() {
      return this.txtElements[0]
    },
    part2() {
      return this.txtElements[2]
    },
    part3() {
      if (this.txtElements.length > 4) {
        return this.txtElements[4]
      } else {
        return ''
      }
    }
  },
  methods: {
    clickUseTerm() {
      this.$emit('openUseTerm')
    },
    clickPrivacyPolicy() {
      this.$emit('openPrivacyPolicy')
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
