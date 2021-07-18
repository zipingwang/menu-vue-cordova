<template>
    <div>
        <Drawer
            :title="ml.customer"
            v-model="show"
             :closable = "false"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
          <div class="row">
              <div class="column" v-for="customer in customers">
                <div class="ricetableblock" @click="openCustomer(customer)">
                      <div class="title">
                          {{customer.firstName}}
                      </div>
                      <div class="title">
                          {{customer.lastName}}
                      </div>
                      <div class="title">
                            {{customer.email}}
                        </div>
                      <div class="title">
                          {{customer.telephone}}
                      </div>
                      <div class="endfilling"></div>
                  </div>
              </div>
            </div> <!--row -->
            <div class="demo-drawer-footer">
              <Button  type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <Button type="primary" @click="addNewCustomer">{{ml.add}}</Button>
          </div>
        </Drawer>
        <customer ref="customer" :ml="ml" :data="data"></customer>
    </div>
</template>
<script>
import customer from 'components/config/customer/customer'

export default {
  components: {
    customer
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
      customers: [],
      selectedCustomer: {}
    }
  },
  created() {
    this.$root.eventHub.$on('signalr.onDownLoadCustomers', this.onDownLoadCustomers)
    this.$root.eventHub.$on('signalr.onDeleteCustomer', this.onDeleteCustomer)
    this.$root.eventHub.$on('signalr.onAddCustomer', this.onAddCustomer)
  },
  mounted() {

  },
  methods: {
    showDraw() {
      this.show = true
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadCustomers'})
    },
    onDownLoadCustomers(messageBody) {
      console.log('onDownLoadCustomers in customers')
      // alert('onDownloadBusinessInfo')
      console.log(messageBody)
      console.log(typeof messageBody)
      this.customers = messageBody
      this.$Message.success(this.ml.success);
    },
    onDeleteCustomer(messageBody) {
      console.log('onDeleteCustomer in customers')
      console.log(messageBody)
      for (var index = 0; index < this.customers.length; index++) {
        if (this.customers[index].rid === messageBody.rid) {
          this.customers.splice(index, 1)
          console.log('customer deleted')
        }
      }
    },
    openCustomer(customer) {
      console.log('open customer')
      this.selectedCustomer = customer
      console.log(this.selectedCustomer)
      this.$refs.customer.showDraw(customer)
    },
    addNewCustomer() {
      var customer = {}
      customer.web = true
      customer.displayOrder = 10
      this.$refs.customer.showDraw(customer)
    },
    onAddCustomer(newCustomer) {
      console.log('onAddCustomer')
      this.customers.push(newCustomer)
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
