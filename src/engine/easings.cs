partial class sploppy {
    const float _c1_ = 1.70158f;
    const float _c3_ = 2.70158f;
    const float _c4_ = 2*pi/3f;
    static float easeoutelastic(float x) => x<=0?0:x>=1?1:max(pow(2,-10*x)*sin((x*10-.75f)*_c4_)+1,0);
    static float easeinback(float x) => x<=0?0:x>=1?1:min(_c3_*x*x*x-_c1_*x*x,1);
}