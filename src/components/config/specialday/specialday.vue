<template>
    <div class="root">
        <Drawer
            :title="ml.specialday"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
         <i-form :model="formSpecialDay" label-position="top" ref="formSpecialDay" :rules="ruleValidate" >
        <form-item :label="ml.from" prop="fromTime">
              <date-picker
                 placeholder="Select date"
                :value="formSpecialDay.fromTime"
                @on-change="handleChangeFrom"
                type="date">
            </date-picker>
        </form-item>
        <form-item :label="ml.to" prop="endTime">
          <date-picker
                 placeholder="Select date"
                :value="formSpecialDay.endTime"
                @on-change="handleChangeTo"
                type="date">
            </date-picker>
        </form-item>
        <form-item :label="ml.iswholedayornot">
            <i-switch v-model="formSpecialDay.isWholeDay" size="large">
                  <span slot="open">{{ml.wholeday}}</span>
                  <span slot="close">{{ml.notwholeday}}</span>
              </i-switch>
        </form-item>
        <form-item :label="ml.selecttime" v-if="formSpecialDay.isWholeDay">
            <time-picker format="HH:mm" :value="formSpecialDay.part1"  @on-change="handleChangePart1" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
            <time-picker format="HH:mm" :value="formSpecialDay.part2"  @on-change="handleChangePart2" :steps="[1, 15, 15]" type="timerange" placement="bottom-end" :placeholder="ml.selecttime" style="width: 168px"></time-picker>
        </form-item>
        <form-item :label="ml.openorclose">
            <i-switch v-model="formSpecialDay.isOpen" size="large">
                  <span slot="open">{{ml.open}}</span>
                  <span slot="close">{{ml.close}}</span>
              </i-switch>
        </form-item>
         <form-item :label="ml.yearlyvacation">
            <i-switch v-model="formSpecialDay.isYearlyVacation" size="large">
                  <span slot="open">{{ml.yearlyvacation}}</span>
                  <span slot="close">{{ml.openingspecialday}}</span>
              </i-switch>
        </form-item>

        <form-item >
          <textarea class="ordercommenttext" v-model="formSpecialDay.comment" rows="2" cols="50%" :placeholder="ml.specialdaycomment"></textarea>
        </form-item>
 </i-form>
          <modal
            ref="dialog"
            v-model="modalDeleteSpecialDay"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteSpecialDay"
            @on-cancel="cancelDeleteSpecialDay">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmdelete}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <sendButton ref="myDeleteButton" :text="ml.delete" :sendingText="ml.sending" :failedText="ml.deletefailed" @click="deleteSpecialDay" style="margin-right: 8px"></sendButton>
              <!-- <Button style="margin-right: 8px" @click="deleteSpecialDay">{{ml.delete}}</Button> -->
              <Button type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveMemuGroup"></sendButton>
              <!-- <Button type="primary" @click="saveMemuGroup">{{ml.save}}</Button> -->
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
      data: {}
      // specialDay: {}
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
        modalDeleteSpecialDay: false,
        formSpecialDay: {},
        // formSpecialDay: {
        //   rid: 0,
        //   displayOrder: 1,
        //   name1: '',
        //   name2: '',
        //   description: '',
        //   web: true,
        //   name3: '',
        //   name4: ''
        // },
        ruleValidate: {
          fromTime: [
            { required: true, message: '*', trigger: 'blur' }
          ],
          endTime: [
            { required: true, message: '*', trigger: 'blur' }
          ]
        }
      }
    },
    created() {
      this.$root.eventHub.$on('signalr.onSaveSpecialDay', this.onSaveSpecialDay)
      this.$root.eventHub.$on('signalr.onDeleteSpecialDay', this.onDeleteSpecialDay)
      // this.$root.eventHub.$on('signalr.onDownLoadSpecialDay', this.onDownLoadSpecialDay)
    },
    mounted() {

    },
    methods: {
      showDraw(specialDay) {
        console.log('show draw')
        this.show = true
        this.formSpecialDay = specialDay
      },
      close() {
        this.show = false
      },
      saveMemuGroup() {
        this.$refs.formSpecialDay.validate((valid) => {
          if (valid) {
            this.$refs.mySendButton.start()
            // this.formSpecialDay['siterid'] = this.data.options.shopRid
            this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveSpecialDay', 'messageBody': this.formSpecialDay})
          } else {
            this.$Message.error(this.ml.formvalidationerror);
          }
        })
      },
      confirmDeleteSpecialDay() {
        this.$refs.myDeleteButton.start()
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteSpecialDay', 'messageBody': this.formSpecialDay})
        this.modalDeleteSpecialDay = false
      },
      cancelDeleteSpecialDay() {
        this.modalDeleteSpecialDay = false
      },
      onSaveSpecialDay(messageBody) {
        this.$refs.mySendButton.stop()
        this.$Message.success(this.ml.savesuccessfully);
        this.formSpecialDay = messageBody
        // console.log(messageBody)
        // console.log(typeof messageBody)
        // console.log(messageBody.name1)
        // console.log(messageBody.displayOrder)
        // console.log(messageBody[0])
        // this.formSpecialDay.name1 = messageBody.name1
        console.log('saved specialday')
        console.log(this.formSpecialDay)
        // alert('done')
      },
      onDownLoadSpecialDay(messageBody) {
        console.log('onDownLoadSpecialDay in specialDay')
        // alert('onDownloadBusinessInfo')
        console.log(messageBody)
        console.log(typeof messageBody)
        // this.formSpecialDay.name1 = messageBody.name1

        this.$Message.success(this.ml.success);
      },
      onDeleteSpecialDay(messageBody) {
        console.log('onDeleteSpecialDay in specialDay')
        this.$refs.myDeleteButton.stop()
        this.$Message.success(this.ml.deletesuccessfully);
        this.close()
      },
      deleteSpecialDay() {
        this.modalDeleteSpecialDay = true
      },
      handleChangeFrom(time) {
        this.formSpecialDay.fromTime = time
      },
      handleChangeTo(time) {
        this.formSpecialDay.endTime = time
      },
      handleChangePart1(time) {
        this.formSpecialDay.part1 = time
      },
      handleChangePart2(time) {
        this.formSpecialDay.part2 = time
      },
      handleUploadImageSuccess(res, file) {
        console.log('uploadimage success')
        file.url = this.avatarUrl
        this.$Message.success(this.ml.success);
      },
      handleFormatError(file) {
        this.$Notice.warning({
          title: '',
          desc: this.ml.fileformatnotcorrect
        });
      },
      handleMaxSize(file) {
        this.$Notice.warning({
          title: '',
          desc: this.ml.filesizetoolarge
        });
      }
    }
  }
</script>
<style>
  .root {
    z-index: 101;
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
