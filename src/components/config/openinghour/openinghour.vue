<template>
    <div>
        <Drawer
            :title="ml.openinghour"
            v-model="show"
            width="100%"
            :closable="false"
            :mask-closable="false"
            :styles="styles"
        >
            <div class="dayBlock">
              <p>{{ml.monday}}</p>
              <div class="row">
                <span class="lable">{{ml.part1}}</span>
                <time-picker format="HH:mm" :value="openinghour.mondayPart1"   @on-change="handleChangeMondayPart1"  :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
              <div class="row">
                <span class="lable">{{ml.part2}}</span>
                <time-picker format="HH:mm" :value="openinghour.mondayPart2"  @on-change="handleChangeMondayPart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
            </div>
            <div class="dayBlock">
              <p>{{ml.tuesday}}</p>
              <div class="row">
                <span class="lable">{{ml.part1}}</span>
                <time-picker format="HH:mm" :value="openinghour.tuesdayPart1"  @on-change="handleChangeTuesdayPart1"  :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
              <div class="row">
                <span class="lable">{{ml.part2}}</span>
                <time-picker format="HH:mm" :value="openinghour.tuesdayPart2"   @on-change="handleChangeTuesdayPart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
            </div>
            <div class="dayBlock">
              <p>{{ml.wensday}}</p>
              <div class="row">
                <span class="lable">{{ml.part1}}</span>
                <time-picker format="HH:mm" :value="openinghour.wensdayPart1"  @on-change="handleChangeWensdayPart1" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
              <div class="row">
                <span class="lable">{{ml.part2}}</span>
                <time-picker format="HH:mm" :value="openinghour.wensdayPart2"  @on-change="handleChangeWensdayPart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
            </div>
            <div class="dayBlock">
              <p>{{ml.thursday}}</p>
              <div class="row">
                <span class="lable">{{ml.part1}}</span>
                <time-picker format="HH:mm" :value="openinghour.thursdayPart1"  @on-change="handleChangeThursdayPart1" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
              <div class="row">
                <span class="lable">{{ml.part2}}</span>
                <time-picker format="HH:mm" :value="openinghour.thursdayPart2"  @on-change="handleChangeThursdayPart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
            </div>
            <div class="dayBlock">
              <p>{{ml.friday}}</p>
              <div class="row">
                <span class="lable">{{ml.part1}}</span>
                <time-picker format="HH:mm" :value="openinghour.fridayPart1"  @on-change="handleChangeFridayPart1"  :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
              <div class="row">
                <span class="lable">{{ml.part2}}</span>
                <time-picker format="HH:mm" :value="openinghour.fridayPart2"  @on-change="handleChangeFridayPart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
            </div>
            <div class="dayBlock">
              <p>{{ml.saterday}}</p>
              <div class="row">
                <span class="lable">{{ml.part1}}</span>
                <time-picker format="HH:mm" :value="openinghour.saterdayPart1"  @on-change="handleChangeSaterdayPart1" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
              <div class="row">
                <span class="lable">{{ml.part2}}</span>
                <time-picker format="HH:mm" :value="openinghour.saterdayPart2"  @on-change="handleChangeSaterdayPart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
            </div>
             <div class="dayBlock">
              <p>{{ml.sunday}}</p>
              <div class="row">
                <span class="lable">{{ml.part1}}</span>
                <time-picker format="HH:mm" :value="openinghour.sundayPart1"  @on-change="handleChangeSundayPart1" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
              <div class="rowEnd">
                <span class="lable">{{ml.part2}}</span>
                <time-picker format="HH:mm" :value="openinghour.sundayPart2"  @on-change="handleChangeSundayPart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
              </div>
            </div>
            <div class="demo-drawer-footer">
                <Button type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
                <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveOpeninghour"></sendButton>
            </div>
        </Drawer>
    </div>
</template>

<script>
import sendButton from 'components/common/sendButton/sendButton'

export default {
  components: {
    sendButton
  },
  props: {
    ml: {},
    data: {},
    seller: {}
  },
  data() {
    return {
      styles: {
        height: 'calc(100% - 55px)',
        overflow: 'auto',
        paddingBottom: '53px',
        position: 'static'
      },
      show: false,
      // openinghour: {
      //   monday{{ml.part1}} ['12:15:00', '14:30:00'],
      //   monday{{ml.part2}} ['18:00:00', '22:00:00']
      // }
      openinghour: {}
    }
  },
  created() {
    this.$root.eventHub.$on('signalr.onSaveOpeninghour', this.onSaveOpeninghour)
    this.$root.eventHub.$on('signalr.onDownLoadOpeninghour', this.onDownLoadOpeninghour)
  },
  methods: {
    showDraw() {
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadOpeninghour'})
      this.show = true
    },
    saveOpeninghour() {
      console.log('saveOpeninghour')
      console.log(this.openinghour)
      this.$refs.mySendButton.start()
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveOpeninghour', 'messageBody': this.openinghour})
    },
    close() {
      this.$emit('closed')
      this.show = false
      console.log('emit closed')
    },
    onSaveOpeninghour() {
      this.$refs.mySendButton.stop()
      this.$Message.success(this.ml.savesuccessfully);
    },
    onDownLoadOpeninghour(messageBody) {
      this.openinghour = messageBody
      console.log(this.openinghour)
      this.$Message.success(this.ml.success);
    },
    handleChangeMondayPart1(time) {
      this.openinghour.mondayPart1 = time
    },
    handleChangeMondayPart2(time) {
      this.openinghour.mondayPart2 = time
    },
    handleChangeTuesdayPart1(time) {
      this.openinghour.tuesdayPart1 = time
    },
    handleChangeTuesdayPart2(time) {
      this.openinghour.tuesdayPart2 = time
    },
    handleChangeWensdayPart1(time) {
      this.openinghour.wensdayPart1 = time
    },
    handleChangeWensdayPart2(time) {
      this.openinghour.wensdayPart2 = time
    },
    handleChangeThursdayPart1(time) {
      this.openinghour.thursdayPart1 = time
    },
    handleChangeThursdayPart2(time) {
      this.openinghour.thursdayPart2 = time
    },
    handleChangeFridayPart1(time) {
      this.openinghour.fridayPart1 = time
    },
    handleChangeFridayPart2(time) {
      this.openinghour.fridayPart2 = time
    },
    handleChangeSaterdayPart1(time) {
      this.openinghour.saterdayPart1 = time
    },
    handleChangeSaterdayPart2(time) {
      this.openinghour.saterdayPart2 = time
    },
    handleChangeSundayPart1(time) {
      this.openinghour.sundayPart1 = time
    },
    handleChangeSundayPart2(time) {
      this.openinghour.sundayPart2 = time
    }
  }
}
</script>
<style>
  .dayblock{
    margin-top: 10px;
  }
  .row{
    margin-top: 10px;
  }
  .rowEnd{
    margin-top: 10px;
    margin-bottom: 40px;
  }
  .timeSlot{
    text-align: left;
  }
  .lable{
    text-align: right;
    margin: 2px 20px;
  }
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
