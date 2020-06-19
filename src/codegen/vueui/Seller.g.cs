 <i-form :model="formSeller" label-position="top" ref="formSeller" :rules="ruleValidate" >
       
        <form-item :label="ml.name">
                <i-input v-model="formSeller.name"></i-input>
        </form-item>
       
        <form-item :label="ml.description">
                <i-input v-model="formSeller.description"></i-input>
        </form-item>
       
        <form-item :label="ml.bulletin">
                <i-input v-model="formSeller.bulletin"></i-input>
        </form-item>
       
        <form-item :label="ml.avatar">
                <i-input v-model="formSeller.avatar"></i-input>
        </form-item>
       
        <form-item :label="ml.openinghour">
                <i-input v-model="formSeller.openingHour"></i-input>
        </form-item>
       
        <form-item :label="ml.address">
                <i-input v-model="formSeller.address"></i-input>
        </form-item>
       
        <form-item :label="ml.telephone">
                <i-input v-model="formSeller.telephone"></i-input>
        </form-item>
       
        <form-item :label="ml.supportonlineorder">
                <i-input v-model="formSeller.supportOnlineOrder"></i-input>
        </form-item>
              
 </i-form>

 formSeller: {

name: '',

description: '',

bulletin: '',

avatar: '',

openingHour: '',

address: '',

telephone: '',

supportOnlineOrder: '',

},

 
      public string name{get;set;}
       
      public string description{get;set;}
       
      public string bulletin{get;set;}
       
      public string avatar{get;set;}
       
      public string openingHour{get;set;}
       
      public string address{get;set;}
       
      public string telephone{get;set;}
       
      public string supportOnlineOrder{get;set;}
              

 ruleValidate: {

          name: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          description: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          bulletin: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          avatar: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          openingHour: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          address: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          telephone: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          supportOnlineOrder: [
              { required: true, message: '*', trigger: 'blur' }
          ]
      
        },

public void Merge(Seller entity)
        {

            this.name = entity.Name;     
  
            this.description = entity.Description;     
  
            this.bulletin = entity.Bulletin;     
  
            this.avatar = entity.Avatar;     
  
            this.openingHour = entity.OpeningHour;     
  
            this.address = entity.Address;     
  
            this.telephone = entity.Telephone;     
  
            this.supportOnlineOrder = entity.SupportOnlineOrder;     
             
        }

  public void MapToEntity(Seller entity)
        {

            entity.Name = name;
  
            entity.Description = description;
  
            entity.Bulletin = bulletin;
  
            entity.Avatar = avatar;
  
            entity.OpeningHour = openingHour;
  
            entity.Address = address;
  
            entity.Telephone = telephone;
  
            entity.SupportOnlineOrder = supportOnlineOrder;
       
        }