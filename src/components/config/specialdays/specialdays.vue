<template>
    <div>
        <Drawer
            :title="ml.specialday"
            v-model="show"
             :closable = "false"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
          <div class="row">
              <div class="column" v-for="specialday in specialDays">
                <div class="ricetableblock" @click="openSpecialDay(specialday)">
                      <!-- <div class="title">
                          {{specialday.description}} // strange this field not refreshed when saved in specialday.vue
                      </div> -->
                      <div class="title">
                          <!-- {{summary(specialday)}} -->
                          <!-- toLocaleDateString to display only date part -->
                          {{new Date(specialday.fromTime).toLocaleDateString()}} - {{new Date(specialday.endTime).toLocaleDateString()}} - {{specialday.isOpen ===true?'Open':'Close'}}
                      </div>
                      <div class="endfilling"></div>
                  </div>
              </div>
            </div> <!--row -->
            <div class="demo-drawer-footer">
              <Button  type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <Button type="primary" @click="addNewSpecialDay">{{ml.add}}</Button>
          </div>
        </Drawer>
        <specialday ref="specialday" :ml="ml" :data="data"></specialday>
    </div>
</template>
<script>

import axios from 'axios'
import specialday from 'components/config/specialday/specialday'

export default {
  components: {
    specialday
  },
  props: {
    ml: {},
    data: {}
  },
  data () {
    return {
      styles: {
        height: 'calc(100% - 55px)',
        overflow: 'auto',
        paddingBottom: '53px',
        position: 'static'
      },
      show: this.visible,
      specialDays: [],
      selectedSpecialDay: {}
    }
  },
  created() {
    this.$root.eventHub.$on('signalr.onDownLoadSpecialDays', this.onDownLoadSpecialDays)
    this.$root.eventHub.$on('signalr.onDeleteSpecialDay', this.onDeleteSpecialDay)
    this.$root.eventHub.$on('signalr.onAddSpecialDay', this.onAddSpecialDay)
  },
  mounted() {

  },
  computed: {
    summary: function () {
      var vm = this;
      return function (specialday) {
        return new Date(specialday.fromTime).toLocaleDateString() + ' - ' + new Date(specialday.endTime).toLocaleDateString() + ' ' + specialday.isOpen === true ? 'Open' : 'Close';
      };
    }
  },
  methods: {
    showDraw() {
      this.show = true
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadSpecialDays'})
      // this.downloadSpecialDays()
    },
    downloadSpecialDays() {
      console.log('downloadSpecialDays')
      var url = this.data.options.requestUrl + 'method=specialdays&siterid=' + this.data.options.shopRid
      axios.get(url).then((res) => {
        console.log('get downloadSpecialDays ok')
        console.log(res)
        // alert(res)
        if (res.data.status === 'ok') {
          this.specialDays = res.data.messageBody
        }
      })
    },
    onDownLoadSpecialDays(messageBody) {
      console.log('onDownLoadSpecialDays in specialDays')
      // alert('onDownloadBusinessInfo')
      console.log(messageBody)
      console.log(typeof messageBody)
      this.specialDays = messageBody
      this.$Message.success(this.ml.success);
    },
    onDeleteSpecialDay(messageBody) {
      console.log('onDeleteSpecialDay in specialdays')
      console.log(messageBody)
      for (var index = 0; index < this.specialDays.length; index++) {
        if (this.specialDays[index].rid === messageBody.rid) {
          this.specialDays.splice(index, 1)
          console.log('specialday deleted')
        }
      }
    },
    openSpecialDay(specialday) {
      console.log('open specialday')
      this.selectedSpecialDay = specialday
      console.log(this.selectedSpecialDay)
      this.$refs.specialday.showDraw(specialday)
    },
    addNewSpecialDay() {
      var specialday = {}
      specialday.isOpen = false
      specialday.isYearlyVacation = false
      this.$refs.specialday.showDraw(specialday)
    },
    onAddSpecialDay(newSpecialDay) {
      console.log('onAddSpecialDay')
      this.specialDays.push(newSpecialDay)
    },
    close() {
      this.show = false
      this.$emit('closed')
    }
  }
}
</script>
<style lang="stylus" scoped>
/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: wrap;
}

/* Create four equal columns */
/* maximum 4 column, if screen width is not enough, it will display less column, e.g. 3 or 2 or 1, new row will made. */
.column {
  flex: 25%;
  padding: 20px;
}
/* On screens that are 992px wide or less, go from four columns to two columns */
@media screen and (max-width: 992px) {
  .column {
    flex: 50%;
  }
}

/* On screens that are 600px wide or less, make the columns stack on top of each other instead of next to each other */
@media screen and (max-width: 600px) {
  .row {
    flex-direction: column;
  }
}

.row
  .column
    text-align center
    color rgb(77,85,93)
    .ricetableblock
      border 1px solid #f3f5f7
      border-radius: 10px
      height: 100%
      box-sizing border-box
      .title
        font-size 24px
        line-height 24px
        margin-top 20px
      .endfilling
        height 20px
.demo-drawer-footer{
    width: 100%;
    position: absolute;
    bottom: 0;
    left: 0;
    border-top: 1px solid #e8e8e8;
    padding: 10px 16px;
    text-align: right;
    background: #fff;
  }

</style>
