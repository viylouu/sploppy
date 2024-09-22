partial class sploppy {
    const float _c1_ = 1.70158f;
    const float _c3_ = 2.70158f;
    const float _c4_ = 2*pi/3f;
    static float easeoutelastic(float x) => x<=0?0:x>=1?1:max(pow(2,-10*x)*sin((x*10-.75f)*_c4_)+1,0);
    static float easeinback(float x) => x<=0?0:x>=1?1:min(_c3_*x*x*x-_c1_*x*x,1);
    static float easeoutquint(float x) => clamp(1-pow(1-x,5),0,1);
    static float easeoutback(float x) => 1+_c3_*pow(clamp(x,0,1)-1,3)+_c1_*pow(clamp(x,0,1)-1,2);
}