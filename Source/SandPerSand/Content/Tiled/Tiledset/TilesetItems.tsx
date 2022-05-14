<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.4" tiledversion="1.4.3" name="TilesetItems" tilewidth="32" tileheight="32" tilecount="64" columns="8">
 <image source="../TiledsetTexture/TilesetItems.png" width="256" height="256"/>
 <tile id="0" type="Item">
  <properties>
   <property name="item_description">Make all players
ahead of you slower,
and less jumpy
for a duration.</property>
   <property name="item_flavour">Kaboom!
Everyone's small!</property>
   <property name="item_id" value="lightning"/>
   <property name="item_name" value="Blitz!"/>
   <property name="item_price" type="int" value="10"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_blitz"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="9.70044" y="5.15575" width="14.4743" height="21.1195"/>
  </objectgroup>
 </tile>
 <tile id="1" type="Item">
  <properties>
   <property name="item_description" value="Coind within a radius will be attracted to you for a duration."/>
   <property name="item_flavour" value="Let the money come to you..."/>
   <property name="item_id" value="magnet"/>
   <property name="item_name" value="Coin Magnet"/>
   <property name="item_price" type="int" value="0"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_unknown"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="2.67335" y="2.74973" width="27.1918" height="27.1918"/>
  </objectgroup>
 </tile>
 <tile id="2" type="Item">
  <properties>
   <property name="item_description" value="Disable the leading player's controls for a duration."/>
   <property name="item_flavour" value="You! Stop right there!"/>
   <property name="item_id" value="ice_block"/>
   <property name="item_name" value="Freeze!"/>
   <property name="item_price" type="int" value="4"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_freeze"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="5.18873" y="4.08293" width="25.4333" height="25.7735"/>
  </objectgroup>
 </tile>
 <tile id="3" type="Item">
  <properties>
   <property name="item_description" value="this is a dummy description"/>
   <property name="item_flavour" value="And he said, &quot;Let there be Sand!&quot;"/>
   <property name="item_id" value="portable_sand_source"/>
   <property name="item_name" value="Summon Sand"/>
   <property name="item_price" type="int" value="0"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_sandsource"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="3.99787" y="2.97714" width="24.2424" height="23.8171"/>
  </objectgroup>
 </tile>
 <tile id="4" type="Item">
  <properties>
   <property name="item_description">Jump the rest of the
way with style and
swagger,
you hot alien!
(This item is
purely cosmetic)</property>
   <property name="item_flavour" value="Deal with it."/>
   <property name="item_id" value="sunglasses"/>
   <property name="item_name" value="Sunglasses"/>
   <property name="item_price" type="int" value="20"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_glasses"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.255183" y="11.9086" width="31.6427" height="5.6991"/>
  </objectgroup>
 </tile>
 <tile id="5" type="Item">
  <properties>
   <property name="item_description">You and and the
player in first place
other than yourself
swap postitions
upon use.</property>
   <property name="item_flavour">Whoops!
You're there now!</property>
   <property name="item_id" value="position_swap"/>
   <property name="item_name" value="Position-Swap"/>
   <property name="item_price" type="int" value="15"/>
   <property name="item_type" value="major"/>
   <property name="item_usage_icon" value="usage_posswap"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.425306" y="4.33812" width="30.7071" height="25.3482"/>
  </objectgroup>
 </tile>
 <tile id="6" type="Item">
  <properties>
   <property name="item_description" value="Increase your maximum jump height by 1.5x for a duration."/>
   <property name="item_flavour" value="Fly! You fool!"/>
   <property name="item_id" value="wings"/>
   <property name="item_name" value="Wings"/>
   <property name="item_price" type="int" value="0"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_wings"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="5.61404" y="5.10367" width="20.0744" height="21.8607"/>
  </objectgroup>
 </tile>
 <tile id="7" type="Item">
  <properties>
   <property name="item_description" value="Increase your maximum horizontal speed by 1.5x"/>
   <property name="item_flavour" value="Zip, zap, zappy."/>
   <property name="item_id" value="speedup"/>
   <property name="item_name" value="Speedup"/>
   <property name="item_price" type="int" value="0"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_speedup"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="8.7613" y="6.88995" width="15.6512" height="17.1823"/>
  </objectgroup>
 </tile>
 <tile id="8" type="Item">
  <properties>
   <property name="item_description">Invert all players'
controls except
your own for a
duration.</property>
   <property name="item_flavour">Side-effects may
include dizziness,
 and vomitting...</property>
   <property name="item_id" value="dizzy_eyes"/>
   <property name="item_name" value="Every-Stun!"/>
   <property name="item_price" type="int" value="10"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_dizzy"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="3.99787" y="4.5933" width="23.9872" height="24.0723"/>
  </objectgroup>
 </tile>
 <tile id="9" type="Item">
  <properties>
   <property name="item_description" value="The next effect targiting you will be countered while carrying this item."/>
   <property name="item_flavour" value="Begone, foe!"/>
   <property name="item_id" value="shield"/>
   <property name="item_name" value="Shield!"/>
   <property name="item_price" type="int" value="0"/>
   <property name="item_type" value="minor"/>
   <property name="item_usage_icon" value="usage_sheild"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="3.99787" y="3.14726" width="24.8379" height="23.2217"/>
  </objectgroup>
 </tile>
 <tile id="10" type="Item"/>
 <tile id="11" type="Item"/>
 <tile id="12" type="Item"/>
 <tile id="13" type="Item"/>
 <tile id="14" type="Item"/>
 <tile id="15" type="Item"/>
 <tile id="16" type="Item"/>
 <tile id="17" type="Item"/>
 <tile id="18" type="Item"/>
 <tile id="19" type="Item"/>
 <tile id="20" type="Item"/>
 <tile id="21" type="Item"/>
 <tile id="22" type="Item"/>
 <tile id="23" type="Item"/>
 <tile id="24" type="Item"/>
 <tile id="25" type="Item"/>
 <tile id="26" type="Item"/>
 <tile id="27" type="Item"/>
 <tile id="28" type="Item"/>
 <tile id="29" type="Item"/>
 <tile id="30" type="Item"/>
 <tile id="31" type="Item"/>
 <tile id="32" type="Item"/>
 <tile id="33" type="Item"/>
 <tile id="34" type="Item"/>
 <tile id="35" type="Item"/>
 <tile id="36" type="Item"/>
 <tile id="37" type="Item"/>
 <tile id="38" type="Item"/>
 <tile id="39" type="Item"/>
 <tile id="40" type="Item"/>
 <tile id="41" type="Item"/>
 <tile id="42" type="Item"/>
 <tile id="43" type="Item"/>
 <tile id="44" type="Item"/>
 <tile id="45" type="Item"/>
 <tile id="46" type="Item"/>
 <tile id="47" type="Item"/>
 <tile id="48" type="Item"/>
 <tile id="49" type="Item"/>
 <tile id="50" type="Item"/>
 <tile id="51" type="Item"/>
 <tile id="52" type="Item"/>
 <tile id="53" type="Item"/>
 <tile id="54" type="Item"/>
 <tile id="55" type="Item"/>
 <tile id="56" type="Item"/>
 <tile id="57" type="Item"/>
 <tile id="58" type="Item"/>
 <tile id="59" type="Item"/>
 <tile id="60" type="Item"/>
 <tile id="61" type="Item"/>
 <tile id="62" type="Item"/>
 <tile id="63" type="Item"/>
</tileset>
